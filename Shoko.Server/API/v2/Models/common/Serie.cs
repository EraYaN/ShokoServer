﻿using Shoko.Models.PlexAndKodi;
using Shoko.Models.Server;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using Nancy;
using Shoko.Commons.Extensions;
using Shoko.Models.Enums;
using Shoko.Server.Models;
using Shoko.Server.PlexAndKodi;
using Shoko.Server.Repositories;

namespace Shoko.Server.API.v2.Models.common
{
    [DataContract]
    public class Serie : BaseDirectory, IComparable
    {
        public override string type => "serie";

        [DataMember(IsRequired = true, EmitDefaultValue = true)]
        public int aid { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string season { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public List<Episode> eps { get; set; }

        [DataMember(IsRequired = true, EmitDefaultValue = true)]
        public int ismovie { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public long filesize { get; set; }

        public Serie()
        {
            art = new ArtCollection();
            roles = new List<Role>();
            tags = new List<string>();
        }

        public static Serie GenerateFromVideoLocal(NancyContext ctx, SVR_VideoLocal vl, int uid, bool nocast, bool notag, int level, bool all, bool allpics, int pic, TagFilter.Filter tagfilter)
        {
            Serie sr = new Serie();

            if (vl == null) return sr;
            var ser = vl.GetAnimeEpisodes().FirstOrDefault()?.GetAnimeSeries();
            if (ser == null) return sr;
            sr = GenerateFromAnimeSeries(ctx, ser, uid, nocast, notag, level, all, allpics, pic, tagfilter);

            return sr;
        }

        public static Serie GenerateFromBookmark(NancyContext ctx, BookmarkedAnime bookmark, int uid, bool nocast, bool notag, int level, bool all, bool allpics, int pic, TagFilter.Filter tagfilter)
        {
            var series = RepoFactory.AnimeSeries.GetByAnimeID(bookmark.AnimeID);
            if (series != null)
                return GenerateFromAnimeSeries(ctx, series, uid, nocast, notag, level, all, allpics, pic, tagfilter);

            SVR_AniDB_Anime aniDB_Anime = RepoFactory.AniDB_Anime.GetByAnimeID(bookmark.AnimeID);
            if (aniDB_Anime == null)
            {
                Commands.CommandRequest_GetAnimeHTTP cr_anime = new Commands.CommandRequest_GetAnimeHTTP(bookmark.AnimeID, true, false);
                cr_anime.Save();

                Serie empty_serie = new Serie
                {
                    id = -1,
                    name = "GetAnimeInfoHTTP",
                    aid = bookmark.AnimeID
                };
                return empty_serie;
            }
            return GenerateFromAniDB_Anime(ctx, aniDB_Anime, nocast, notag, allpics, pic, tagfilter);
        }

        public static Serie GenerateFromAniDB_Anime(NancyContext ctx, SVR_AniDB_Anime anime, bool nocast, bool notag, bool allpics, int pic, TagFilter.Filter tagfilter)
        {
            Serie sr = new Serie
            {
                // 0 will load all
                id = -1,
                aid = anime.AnimeID,
                summary = anime.Description,
                rating = Math.Round(anime.Rating / 100D, 1)
                    .ToString(CultureInfo.InvariantCulture),
                votes = anime.VoteCount.ToString(),
                name = anime.MainTitle,
                ismovie = anime.AnimeType == (int) AnimeType.Movie ? 1 : 0
            };

            if (anime.AirDate != null)
            {
                sr.year = anime.AirDate.Value.Year.ToString();
                var airdate = anime.AirDate.Value;
                if (airdate != DateTime.MinValue)
                    sr.air = airdate.ToPlexDate();
            }

            AniDB_Vote vote = RepoFactory.AniDB_Vote.GetByEntityAndType(anime.AnimeID, AniDBVoteType.Anime) ??
                              RepoFactory.AniDB_Vote.GetByEntityAndType(anime.AnimeID, AniDBVoteType.AnimeTemp);
            if (vote != null)
                sr.userrating = Math.Round(vote.VoteValue / 100D, 1).ToString(CultureInfo.InvariantCulture);
            sr.titles = anime.GetTitles().Select(title =>
                new AnimeTitle {Language = title.Language, Title = title.Title, Type = title.TitleType}).ToList();

            PopulateArtFromAniDBAnime(ctx, anime, sr, allpics, pic);

            if (!nocast)
            {
                var xref_animestaff = RepoFactory.CrossRef_Anime_Staff.GetByAnimeIDAndRoleType(anime.AnimeID,
                    StaffRoleType.Seiyuu);
                foreach (var xref in xref_animestaff)
                {
                    if (xref.RoleID == null) continue;
                    var character = RepoFactory.AnimeCharacter.GetByID(xref.RoleID.Value);
                    if (character == null) continue;
                    var staff = RepoFactory.AnimeStaff.GetByID(xref.StaffID);
                    if (staff == null) continue;
                    var role = new Role
                    {
                        character = character.Name,
                        character_image = APIHelper.ConstructImageLinkFromTypeAndId(ctx, (int) ImageEntityType.Character,
                            xref.RoleID.Value),
                        staff = staff.Name,
                        staff_image = APIHelper.ConstructImageLinkFromTypeAndId(ctx, (int) ImageEntityType.Staff,
                            xref.StaffID),
                        role = xref.Role,
                        type = ((StaffRoleType) xref.RoleType).ToString()
                    };
                    if (sr.roles == null) sr.roles = new List<Role>();
                    sr.roles.Add(role);
                }
            }

            if (!notag)
            {
                var tags = anime.GetAllTags();
                if (tags != null)
                    sr.tags = TagFilter.ProcessTags(tagfilter, tags.ToList());
            }

            return sr;
        }

        public static Serie GenerateFromAnimeSeries(NancyContext ctx, SVR_AnimeSeries ser, int uid, bool nocast, bool notag, int level, bool all, bool allpics, int pic, TagFilter.Filter tagfilter)
        {
            Serie sr = GenerateFromAniDB_Anime(ctx, ser.GetAnime(), nocast, notag, allpics, pic, tagfilter);

            List<SVR_AnimeEpisode> ael = ser.GetAnimeEpisodes();
            var contract = ser.Contract;
            if (contract == null) ser.UpdateContract();

            sr.id = ser.AnimeSeriesID;
            sr.name = ser.GetSeriesName();
            GenerateSizes(sr, ael, uid);

            int? season = ael.FirstOrDefault(a =>
                    a.AniDB_Episode.EpisodeType == (int) EpisodeType.Episode && a.AniDB_Episode.EpisodeNumber == 1)
                ?.TvDBEpisode?.SeasonNumber;
            if (season != null)
                sr.season = season.Value.ToString();

            if (level > 0)
            {
                if (ael.Count > 0)
                {
                    sr.eps = new List<Episode>();
                    foreach (SVR_AnimeEpisode ae in ael)
                    {
                        if (!all && (ae?.GetVideoLocals()?.Count ?? 0) == 0) continue;
                        Episode new_ep = Episode.GenerateFromAnimeEpisode(ctx, ae, uid, (level - 1), pic);
                        if (new_ep == null) continue;

                        sr.eps.Add(new_ep);

                        if (level - 1 <= 0) continue;
                        foreach (RawFile file in new_ep.files) sr.filesize += file.size;
                    }
                    sr.eps = sr.eps.OrderBy(a => a.epnumber).ToList();
                }
            }

            return sr;
        }

        private static void GenerateSizes(Serie sr, List<SVR_AnimeEpisode> ael, int uid)
        {
            int eps = 0;
            int credits = 0;
            int specials = 0;
            int trailers = 0;
            int parodies = 0;
            int others = 0;

            int local_eps = 0;
            int local_credits = 0;
            int local_specials = 0;
            int local_trailers = 0;
            int local_parodies = 0;
            int local_others = 0;

            int watched_eps = 0;
            int watched_credits = 0;
            int watched_specials = 0;
            int watched_trailers = 0;
            int watched_parodies = 0;
            int watched_others = 0;

            // single loop. Will help on long shows
            foreach (SVR_AnimeEpisode ep in ael)
            {
                if (ep == null) continue;
                var local = ep.GetVideoLocals().Any();
                bool watched = (ep.GetUserRecord(uid)?.WatchedCount ?? 0) > 0;
                switch (ep.EpisodeTypeEnum)
                {
                    case EpisodeType.Episode:
                    {
                        eps++;
                        if (local) local_eps++;
                        if (watched) watched_eps++;
                        break;
                    }
                    case EpisodeType.Credits:
                    {
                        credits++;
                        if (local) local_credits++;
                        if (watched) watched_credits++;
                        break;
                    }
                    case EpisodeType.Special:
                    {
                        specials++;
                        if (local) local_specials++;
                        if (watched) watched_specials++;
                        break;
                    }
                    case EpisodeType.Trailer:
                    {
                        trailers++;
                        if (local) local_trailers++;
                        if (watched) watched_trailers++;
                        break;
                    }
                    case EpisodeType.Parody:
                    {
                        parodies++;
                        if (local) local_parodies++;
                        if (watched) watched_parodies++;
                        break;
                    }
                    case EpisodeType.Other:
                    {
                        others++;
                        if (local) local_others++;
                        if (watched) watched_others++;
                        break;
                    }
                }
            }

            sr.size = eps + credits + specials + trailers + parodies + others;
            sr.localsize = local_eps + local_credits + local_specials + local_trailers + local_parodies + local_others;
            sr.viewed = watched_eps + watched_credits + watched_specials + watched_trailers + watched_parodies + watched_others;

            sr.total_sizes = new Sizes()
            {
                Episodes = eps,
                Credits = credits,
                Specials = specials,
                Trailers = trailers,
                Parodies = parodies,
                Others = others
            };

            sr.local_sizes = new Sizes()
            {
                Episodes = local_eps,
                Credits = local_credits,
                Specials = local_specials,
                Trailers = local_trailers,
                Parodies = local_parodies,
                Others = local_others
            };

            sr.watched_sizes = new Sizes()
            {
                Episodes = watched_eps,
                Credits = watched_credits,
                Specials = watched_specials,
                Trailers = watched_trailers,
                Parodies = watched_parodies,
                Others = watched_others
            };
        }

        public static void PopulateArtFromAniDBAnime(NancyContext ctx, SVR_AniDB_Anime anime, Serie sr, bool allpics, int pic)
        {
            Random rand = new Random();
            if (allpics || pic > 1)
            {
                if (allpics)
                {
                    pic = 999;
                }

                int pic_index = 0;
                if (anime.AllPosters != null)
                    foreach (var cont_image in anime.AllPosters)
                    {
                        if (pic_index < pic)
                        {
                            sr.art.thumb.Add(new Art()
                            {
                                url = APIHelper.ConstructImageLinkFromTypeAndId(ctx, cont_image.ImageType,
                                    cont_image.AniDB_Anime_DefaultImageID),
                                index = pic_index
                            });
                            pic_index++;
                        }
                        else
                        {
                            break;
                        }
                    }

                pic_index = 0;
                if (anime.Contract.AniDBAnime.Fanarts != null)
                    foreach (var cont_image in anime.Contract.AniDBAnime.Fanarts)
                    {
                        if (pic_index < pic)
                        {
                            sr.art.fanart.Add(new Art()
                            {
                                url = APIHelper.ConstructImageLinkFromTypeAndId(ctx, cont_image.ImageType,
                                    cont_image.AniDB_Anime_DefaultImageID),
                                index = pic_index
                            });
                            pic_index++;
                        }
                        else
                        {
                            break;
                        }
                    }

                pic_index = 0;
                if (anime.Contract.AniDBAnime.Banners != null)
                    foreach (var cont_image in anime.Contract.AniDBAnime.Banners)
                    {
                        if (pic_index < pic)
                        {
                            sr.art.banner.Add(new Art()
                            {
                                url = APIHelper.ConstructImageLinkFromTypeAndId(ctx, cont_image.ImageType,
                                    cont_image.AniDB_Anime_DefaultImageID),
                                index = pic_index
                            });
                            pic_index++;
                        }
                        else
                        {
                            break;
                        }
                    }
            }
            else if (pic > 0)
            {
                sr.art.thumb.Add(new Art()
                {
                    url = APIHelper.ConstructImageLinkFromTypeAndId(ctx, (int) ImageEntityType.AniDB_Cover,
                        anime.AnimeID),
                    index = 0
                });

                var fanarts = anime.Contract.AniDBAnime.Fanarts;
                if (fanarts != null && fanarts.Count > 0)
                {
                    var art = fanarts[rand.Next(fanarts.Count)];
                    sr.art.fanart.Add(new Art()
                    {
                        url = APIHelper.ConstructImageLinkFromTypeAndId(ctx, art.ImageType,
                            art.AniDB_Anime_DefaultImageID),
                        index = 0
                    });
                }

                fanarts = anime.Contract.AniDBAnime.Banners;
                if (fanarts != null && fanarts.Count > 0)
                {
                    var art = fanarts[rand.Next(fanarts.Count)];
                    sr.art.banner.Add(new Art()
                    {
                        url = APIHelper.ConstructImageLinkFromTypeAndId(ctx, art.ImageType,
                            art.AniDB_Anime_DefaultImageID),
                        index = 0
                    });
                }
            }
        }

        public int CompareTo(object obj)
        {
            Serie a = obj as Serie;
            if (a == null) return 1;
            // try year first, as it is more likely to have relevannt data
            if (int.TryParse(a.year, out int s1) && int.TryParse(year, out int s))
            {
                if (s < s1) return -1;
                if (s > s1) return 1;
            }
            // Does it have an air date? Sort by it
            if (!string.IsNullOrEmpty(a.air) && !a.air.Equals(DateTime.MinValue.ToString("dd-MM-yyyy")) &&
                !string.IsNullOrEmpty(air) && !air.Equals(DateTime.MinValue.ToString("dd-MM-yyyy")))
            {
                if (DateTime.TryParseExact(a.air, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out DateTime d1) &&
                    DateTime.TryParseExact(air, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d))
                {
                    if (d < d1) return -1;
                    if (d > d1) return 1;
                }
            }
            // I don't trust TvDB well enough to sort by them. Bakamonogatari...
            // Does it have a Season? Sort by it
            if (int.TryParse(a.season, out s1) && int.TryParse(season, out s))
            {
                // Only try if the season is valid
                if (s >= 0 && s1 >= 0)
                {
                    // Specials
                    if (s == 0 && s1 > 0) return 1;
                    if (s > 0 && s1 == 0) return -1;
                    // Normal
                    if (s < s1) return -1;
                    if (s > s1) return 1;
                }
            }
            return String.Compare(name, a.name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
