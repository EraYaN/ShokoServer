﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shoko.Commons.Extensions;
using Shoko.Models.Client;
using Shoko.Models.Enums;
using Shoko.Models.Interfaces;
using Shoko.Models.Server;
using Shoko.Models.Server.CrossRef;
using Shoko.Models.TvDB;
using Shoko.Models.WebCache;
using Shoko.Server.CommandQueue.Commands.Trakt;
using Shoko.Server.CommandQueue.Commands.TvDB;

using Shoko.Server.Extensions;
using Shoko.Server.Models;
using Shoko.Server.Providers.MovieDB;
using Shoko.Server.Providers.TraktTV;
using Shoko.Server.Providers.TraktTV.Contracts;
using Shoko.Server.Providers.TraktTV.Contracts.Search;
using Shoko.Server.Providers.TvDB;
using Shoko.Server.Providers.WebCache;
using Shoko.Server.Repositories;
using Shoko.Server.Settings;

namespace Shoko.Server
{
    public partial class ShokoServiceImplementation : IShokoServer
    {
        [HttpGet("AniDB/CrossRef/{animeID}")]
        public CL_AniDB_AnimeCrossRefs GetCrossRefDetails(int animeID)
        {
            CL_AniDB_AnimeCrossRefs result = new CL_AniDB_AnimeCrossRefs
            {
                CrossRef_AniDB_TvDB = new List<CrossRef_AniDB_TvDBV2>(),
                TvDBSeries = new List<TvDB_Series>(),
                TvDBEpisodes = new List<TvDB_Episode>(),
                TvDBImageFanarts = new List<TvDB_ImageFanart>(),
                TvDBImagePosters = new List<TvDB_ImagePoster>(),
                TvDBImageWideBanners = new List<TvDB_ImageWideBanner>(),

                CrossRef_AniDB_MovieDB = null,
                MovieDBMovie = null,
                MovieDBFanarts = new List<MovieDB_Fanart>(),
                MovieDBPosters = new List<MovieDB_Poster>(),

                CrossRef_AniDB_MAL = null,

                CrossRef_AniDB_Trakt = new List<CrossRef_AniDB_TraktV2>(),
                TraktShows = new List<CL_Trakt_Show>(),
                AnimeID = animeID
            };

            try
            {
                SVR_AniDB_Anime anime = Repo.Instance.AniDB_Anime.GetByID(animeID);
                if (anime == null) return result;

                var xrefs = Repo.Instance.CrossRef_AniDB_Provider.GetTvDBV2LinksFromAnime(animeID);

                // TvDB
                result.CrossRef_AniDB_TvDB = xrefs;

                foreach (TvDB_Episode ep in anime.GetTvDBEpisodes())
                    result.TvDBEpisodes.Add(ep);

                foreach (var xref in xrefs.DistinctBy(a => a.TvDBID))
                {
                    TvDB_Series ser = Repo.Instance.TvDB_Series.GetByTvDBID(xref.TvDBID);
                    if (ser != null)
                        result.TvDBSeries.Add(ser);

                    foreach (TvDB_ImageFanart fanart in Repo.Instance.TvDB_ImageFanart.GetBySeriesID(xref.TvDBID))
                        result.TvDBImageFanarts.Add(fanart);

                    foreach (TvDB_ImagePoster poster in Repo.Instance.TvDB_ImagePoster.GetBySeriesID(xref.TvDBID))
                        result.TvDBImagePosters.Add(poster);

                    foreach (TvDB_ImageWideBanner banner in Repo.Instance.TvDB_ImageWideBanner.GetBySeriesID(xref
                        .TvDBID))
                        result.TvDBImageWideBanners.Add(banner);
                }

                // Trakt


                foreach (CrossRef_AniDB_TraktV2 xref in anime.GetCrossRefTraktV2())
                {
                    result.CrossRef_AniDB_Trakt.Add(xref);

                    Trakt_Show show = Repo.Instance.Trakt_Show.GetByTraktSlug(xref.TraktID);
                    if (show != null) result.TraktShows.Add(show.ToClient());
                }


                // MovieDB
                SVR_CrossRef_AniDB_Provider xrefMovie = anime.GetCrossRefMovieDB();
                result.CrossRef_AniDB_MovieDB = xrefMovie;


                result.MovieDBMovie = anime.GetMovieDBMovie();


                foreach (MovieDB_Fanart fanart in anime.GetMovieDBFanarts())
                {
                    if (fanart.ImageSize.Equals(Shoko.Models.Constants.MovieDBImageSize.Original,
                        StringComparison.InvariantCultureIgnoreCase))
                        result.MovieDBFanarts.Add(fanart);
                }

                foreach (MovieDB_Poster poster in anime.GetMovieDBPosters())
                {
                    if (poster.ImageSize.Equals(Shoko.Models.Constants.MovieDBImageSize.Original,
                        StringComparison.InvariantCultureIgnoreCase))
                        result.MovieDBPosters.Add(poster);
                }

                // MAL
                List<SVR_CrossRef_AniDB_Provider> xrefMAL = anime.GetCrossRefMAL();
                if (xrefMAL == null)
                    result.CrossRef_AniDB_MAL = null;
                else
                {
                    result.CrossRef_AniDB_MAL = new List<CrossRef_AniDB_Provider>();
                    foreach (SVR_CrossRef_AniDB_Provider xrefTemp in xrefMAL)
                        result.CrossRef_AniDB_MAL.Add(xrefTemp);
                }
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return result;
            }
        }

        #region Web Cache Admin

        [HttpGet("WebCache/IsAdmin")]
        public bool IsWebCacheAdmin()
        {
            try
            {
                string res = WebCacheAPI.Admin_AuthUser();
                return string.IsNullOrEmpty(res);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return false;
            }
        }

        [HttpGet("WebCache/RandomLinkForApproval/{linkType}")]
        public WebCache_AnimeLink Admin_GetRandomLinkForApproval(int linkType)
        {
            try
            {
                AzureLinkType lType = (AzureLinkType) linkType;
                WebCache_AnimeLink link = null;

                switch (lType)
                {
                    case AzureLinkType.TvDB:
                        link = WebCacheAPI.Admin_GetRandomTvDBLinkForApproval();
                        break;
                    case AzureLinkType.Trakt:
                        link = WebCacheAPI.Admin_GetRandomTraktLinkForApproval();
                        break;
                }


                return link;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return null;
            }
        }

        [HttpGet("WebCache/AdminMessages")]
        public List<WebCache_AdminMessage> GetAdminMessages()
        {
            try
            {
                return ServerInfo.Instance.AdminMessages?.ToList() ?? new List<WebCache_AdminMessage>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return new List<WebCache_AdminMessage>();
            }
        }

        #region Admin - TvDB

        [HttpPost("WebCache/CrossRef/TvDB/{crossRef_AniDB_TvDBId}")]
        public string ApproveTVDBCrossRefWebCache(int crossRef_AniDB_TvDBId)
        {
            try
            {
                return WebCacheAPI.Admin_Approve_CrossRefAniDBTvDB(crossRef_AniDB_TvDBId);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return null;
            }
        }

        [HttpDelete("WebCache/CrossRef/TvDB/{crossRef_AniDB_TvDBId}")]
        public string RevokeTVDBCrossRefWebCache(int crossRef_AniDB_TvDBId)
        {
            try
            {
                return WebCacheAPI.Admin_Revoke_CrossRefAniDBTvDB(crossRef_AniDB_TvDBId);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Sends the current user's TvDB links to the web cache, and then admin approves them
        /// </summary>
        /// <returns></returns>
        [HttpPost("WebCache/TvDB/UseLinks/{animeID}")]
        public string UseMyTvDBLinksWebCache(int animeID)
        {
            try
            {
                // Get all the links for this user and anime
                List<CrossRef_AniDB_TvDB> xrefs = Repo.Instance.CrossRef_AniDB_TvDB.GetByAnimeID(animeID);
                if (xrefs == null) return "No Links found to use";

                SVR_AniDB_Anime anime = Repo.Instance.AniDB_Anime.GetByID(animeID);
                if (anime == null) return "Anime not found";

                // make sure the user doesn't alreday have links
                List<WebCache_CrossRef_AniDB_TvDB> results =
                    WebCacheAPI.Admin_Get_CrossRefAniDBTvDB(animeID);
                bool foundLinks = false;
                if (results != null)
                {
                    foreach (WebCache_CrossRef_AniDB_TvDB xref in results)
                    {
                        if (xref.Username.Equals(ServerSettings.Instance.AniDb.Username,
                            StringComparison.InvariantCultureIgnoreCase))
                        {
                            foundLinks = true;
                            break;
                        }
                    }
                }
                if (foundLinks) return "Links already exist, please approve them instead";

                // send the links to the web cache
                foreach (CrossRef_AniDB_TvDB xref in xrefs)
                {
                    WebCacheAPI.Send_CrossRefAniDBTvDB(xref.ToV2Model(), anime.MainTitle);
                }

                // now get the links back from the cache and approve them
                results = WebCacheAPI.Admin_Get_CrossRefAniDBTvDB(animeID);
                if (results != null)
                {
                    List<WebCache_CrossRef_AniDB_TvDB> linksToApprove =
                        new List<WebCache_CrossRef_AniDB_TvDB>();
                    foreach (WebCache_CrossRef_AniDB_TvDB xref in results)
                    {
                        if (xref.Username.Equals(ServerSettings.Instance.AniDb.Username,
                            StringComparison.InvariantCultureIgnoreCase))
                            linksToApprove.Add(xref);
                    }

                    foreach (WebCache_CrossRef_AniDB_TvDB xref in linksToApprove)
                    {
                        WebCacheAPI.Admin_Approve_CrossRefAniDBTvDB(
                            xref.CrossRef_AniDB_TvDBV2ID);
                    }
                    return "Success";
                }
                else
                    return "Failure to send links to web cache";
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return ex.Message;
            }
        }

        #endregion

        #region Admin - Trakt

        [HttpPost("WebCache/CrossRef/Trakt/{crossRef_AniDB_TraktId}")]
        public string ApproveTraktCrossRefWebCache(int crossRef_AniDB_TraktId)
        {
            try
            {
                return WebCacheAPI.Admin_Approve_CrossRefAniDBTrakt(crossRef_AniDB_TraktId);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return null;
            }
        }

        [HttpDelete("WebCache/CrossRef/Trakt/{crossRef_AniDB_TraktId}")]
        public string RevokeTraktCrossRefWebCache(int crossRef_AniDB_TraktId)
        {
            try
            {
                return WebCacheAPI.Admin_Revoke_CrossRefAniDBTrakt(crossRef_AniDB_TraktId);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Sends the current user's Trakt links to the web cache, and then admin approves them
        /// </summary>
        /// <returns></returns>
        [HttpPost("WebCache/Trakt/UseLinks/{animeID}")]
        public string UseMyTraktLinksWebCache(int animeID)
        {
            try
            {
                //NO OP Till webcache working

                // Get all the links for this user and anime

                /*
                List<CrossRef_AniDB_TraktV2> xrefs = Repo.Instance.CrossRef_AniDB_TraktV2.GetByAnimeID(animeID);
                if (xrefs == null) return "No Links found to use";

                SVR_AniDB_Anime anime = Repo.Instance.AniDB_Anime.GetByID(animeID);
                if (anime == null) return "Anime not found";

                // make sure the user doesn't alreday have links
                List<WebCache_CrossRef_AniDB_Trakt> results =
                    WebCacheAPI.Admin_Get_CrossRefAniDBTrakt(animeID);
                bool foundLinks = false;
                if (results != null)
                {
                    foreach (WebCache_CrossRef_AniDB_Trakt xref in results)
                    {
                        if (xref.Username.Equals(ServerSettings.Instance.AniDb.Username,
                            StringComparison.InvariantCultureIgnoreCase))
                        {
                            foundLinks = true;
                            break;
                        }
                    }
                }
                if (foundLinks) return "Links already exist, please approve them instead";

                // send the links to the web cache
                foreach (CrossRef_AniDB_TraktV2 xref in xrefs)
                {
                    WebCacheAPI.Send_CrossRefAniDBTrakt(xref, anime.MainTitle);
                }

                // now get the links back from the cache and approve them
                results = WebCacheAPI.Admin_Get_CrossRefAniDBTrakt(animeID);
                if (results != null)
                {
                    List<WebCache_CrossRef_AniDB_Trakt> linksToApprove =
                        new List<WebCache_CrossRef_AniDB_Trakt>();
                    foreach (WebCache_CrossRef_AniDB_Trakt xref in results)
                    {
                        if (xref.Username.Equals(ServerSettings.Instance.AniDb.Username,
                            StringComparison.InvariantCultureIgnoreCase))
                            linksToApprove.Add(xref);
                    }

                    foreach (WebCache_CrossRef_AniDB_Trakt xref in linksToApprove)
                    {
                        WebCacheAPI.Admin_Approve_CrossRefAniDBTrakt(
                            xref.CrossRef_AniDB_TraktV2ID);
                    }
                    return "Success";
                }
                else
                    return "Failure to send links to web cache";
                */
                //return JMMServer.Providers.Azure.AzureWebAPI.Admin_Approve_CrossRefAniDBTrakt(crossRef_AniDB_TraktId);
                return "Success";
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return ex.Message;
            }
        }

        #endregion

        #endregion

        #region TvDB

        [HttpPost("Serie/TvDB/Refresh/{seriesID}")]
        public string UpdateTvDBData(int seriesID)
        {
            try
            {
                CommandQueue.Queue.Instance.Add(new CmdTvDBUpdateSeries(seriesID, true));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
            }
            return string.Empty;
        }

        [HttpGet("TvDB/Language")]
        public List<TvDB_Language> GetTvDBLanguages()
        {
            try
            {
                return TvDBApiHelper.GetLanguages();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
            }

            return new List<TvDB_Language>();
        }

        [HttpGet("WebCache/CrossRef/TvDB/{animeID}/{isAdmin}")]
        public List<CL_CrossRef_AniDB_Provider> GetTVDBCrossRefWebCache(int animeID, bool isAdmin)
        {
            try
            {
                if (isAdmin)
                    return WebCacheAPI.Admin_Get_CrossRefAniDBTvDB(animeID);
                else
                    return WebCacheAPI.Get_CrossRefAniDBTvDB(animeID);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return new List<WebCache_CrossRef_AniDB_TvDB>();
            }
        }

        [HttpGet("TvDB/CrossRef/{animeID}")]
        public List<CrossRef_AniDB_TvDBV2> GetTVDBCrossRefV2(int animeID)
        {
            try
            {
                return Repo.Instance.CrossRef_AniDB_Provider.GetTvDBV2LinksFromAnime(animeID);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return null;
            }
        }

        [HttpGet("TvDB/CrossRef/Preview/{animeID}/{tvdbID}")]
        public List<CrossRef_AniDB_ProviderEpisode> GetTvDBEpisodeMatchPreview(int animeID, int tvdbID)
        {
            return TvDBLinkingHelper.GetMatchPreviewWithOverrides(animeID, tvdbID);
        }

        [HttpGet("TvDB/CrossRef/Episode/{animeID}")]
        public List<CrossRef_AniDB_ProviderEpisode> GetTVDBCrossRefEpisode(int animeID)
        {
            try
            {
                return Repo.Instance.CrossRef_AniDB_Provider.GetByAnimeIDAndType(animeID,CrossRefType.TvDB).SelectMany(a=>a.EpisodesListOverride.Episodes).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return null;
            }
        }

        [HttpGet("TvDB/Search/{criteria}")]
        public List<TVDB_Series_Search_Response> SearchTheTvDB(string criteria)
        {
            try
            {
                return TvDBApiHelper.SearchSeries(criteria);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return new List<TVDB_Series_Search_Response>();
            }
        }

        [HttpGet("Serie/Seasons/{seriesID}")]
        public List<int> GetSeasonNumbersForSeries(int seriesID)
        {
            List<int> seasonNumbers = new List<int>();
            try
            {
                // refresh data from TvDB
                TvDBApiHelper.UpdateSeriesInfoAndImages(seriesID, true, false);

                seasonNumbers = Repo.Instance.TvDB_Episode.GetSeasonNumbersForSeries(seriesID);

                return seasonNumbers;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return seasonNumbers;
            }
        }

        [HttpPost("TvDB/CrossRef")]
        public string LinkAniDBTvDB(CrossRef_AniDB_TvDBV2 link)
        {
            try
            {
               ;

                if (Repo.Instance.CrossRef_AniDB_Provider.GetByAnimeIdAndProvider(CrossRefType.TvDB, link.AnimeID, link.TvDBID.ToString()).Count > 0 && link.IsAdditive)
                {
                    string msg = $"You have already linked Anime ID {link.AnimeID} to this TvDB show/season/ep";
                    SVR_AniDB_Anime anime = Repo.Instance.AniDB_Anime.GetByID(link.AnimeID);
                    if (anime != null)
                        msg =
                            $"You have already linked Anime {anime.MainTitle} ({link.AnimeID}) to this TvDB show/season/ep";
                    return msg;
                }

                // we don't need to proactively remove the link here anymore, as all links are removed when it is not marked as additive
                CommandQueue.Queue.Instance.Add(new CmdTvDBLinkAniDB(link.AnimeID, link.TvDBID, link.IsAdditive));

                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return ex.Message;
            }
        }
        [HttpPost("TvDB/CrossRef/FromWebCache")]
        public string LinkTvDBUsingWebCacheLinks(List<CrossRef_AniDB_TvDBV2> links)
        {
            //NO OP Need New webCACHE
            /*
            try
            {
                if (links.Count == 0) return "No links were given in the request. This is a bug.";

                var link = links[0];

                var existingLinks = Repo.Instance.CrossRef_AniDB_TvDB.GetByAnimeID(link.AnimeID);
                Repo.Instance.CrossRef_AniDB_TvDB.Delete(existingLinks);
                Repo.Instance.CrossRef_AniDB_TvDB_Episode.DeleteAllUnverifiedLinksForAnime(link.AnimeID);

                // we don't need to proactively remove the link here anymore, as all links are removed when it is not marked as additive
                CommandQueue.Queue.Instance.Add(new CmdTvDBLinkAniDB(link.AnimeID, link.TvDBID, link.IsAdditive));

                var overrides = TvDBLinkingHelper.GetSpecialsOverridesFromLegacy(links);
                foreach (var episodeOverride in overrides)
                {
                    var exists =
                        Repo.Instance.CrossRef_AniDB_TvDB_Episode_Override.GetByAniDBAndTvDBEpisodeIDs(
                            episodeOverride.AniDBEpisodeID, episodeOverride.TvDBEpisodeID);
                    if (exists != null) continue;
                    Repo.Instance.CrossRef_AniDB_TvDB_Episode_Override.Touch(() => episodeOverride);
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return ex.Message;
            }
            }
            */
            return string.Empty;
        
        }

        [HttpPost("TvDB/CrossRef/Episode/{aniDBID}/{tvDBID}")]
        public string LinkAniDBTvDBEpisode(int aniDBID, int tvDBID)
        {
            try
            {
                TvDBApiHelper.LinkAniDBTvDBEpisode(aniDBID, tvDBID);

                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return ex.Message;
            }
        }

        /// <summary>
        /// Removes all tvdb links for one anime
        /// </summary>
        /// <param name="animeID"></param>
        /// <returns></returns>
        [HttpDelete("TvDB/CrossRef/{animeID}")]
        public string RemoveLinkAniDBTvDBForAnime(int animeID)
        {
            try
            {
                SVR_AnimeSeries ser = Repo.Instance.AnimeSeries.GetByAnimeID(animeID);

                if (ser == null) return "Could not find Series for Anime!";

                List<SVR_CrossRef_AniDB_Provider> xrefs = Repo.Instance.CrossRef_AniDB_Provider.GetByAnimeIDAndType(animeID,CrossRefType.TvDB);
                if (xrefs == null) return string.Empty;

                foreach (SVR_CrossRef_AniDB_Provider xref in xrefs)
                {
                    int tvid = int.Parse(xref.CrossRefID);
                    // check if there are default images used associated
                    List<AniDB_Anime_DefaultImage> images = Repo.Instance.AniDB_Anime_DefaultImage.GetByAnimeID(animeID);
                    foreach (AniDB_Anime_DefaultImage image in images)
                    {
                        if (image.ImageParentType == (int) ImageEntityType.TvDB_Banner ||
                            image.ImageParentType == (int) ImageEntityType.TvDB_Cover ||
                            image.ImageParentType == (int) ImageEntityType.TvDB_FanArt)
                        {
                            if (image.ImageParentID == tvid)
                                Repo.Instance.AniDB_Anime_DefaultImage.Delete(image.AniDB_Anime_DefaultImageID);
                        }
                    }

                    TvDBApiHelper.RemoveLinkAniDBTvDB(xref.AnimeID, tvid);
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return ex.Message;
            }
        }

        [HttpDelete("TvDB/CrossRef")]
        public string RemoveLinkAniDBTvDB(CrossRef_AniDB_TvDBV2 link)
        {
            try
            {
                SVR_AnimeSeries ser = Repo.Instance.AnimeSeries.GetByAnimeID(link.AnimeID);

                if (ser == null) return "Could not find Series for Anime!";

                // check if there are default images used associated
                List<AniDB_Anime_DefaultImage> images = Repo.Instance.AniDB_Anime_DefaultImage.GetByAnimeID(link.AnimeID);
                foreach (AniDB_Anime_DefaultImage image in images)
                {
                    if (image.ImageParentType == (int) ImageEntityType.TvDB_Banner ||
                        image.ImageParentType == (int) ImageEntityType.TvDB_Cover ||
                        image.ImageParentType == (int) ImageEntityType.TvDB_FanArt)
                    {
                        if (image.ImageParentID == link.TvDBID)
                            Repo.Instance.AniDB_Anime_DefaultImage.Delete(image.AniDB_Anime_DefaultImageID);
                    }
                }

                TvDBApiHelper.RemoveLinkAniDBTvDB(link.AnimeID, link.TvDBID);

                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return ex.Message;
            }
        }

        [HttpDelete("TvDB/CrossRef/Episode/{aniDBEpisodeID}")]
        public string RemoveLinkAniDBTvDBEpisode(int aniDBEpisodeID, int tvDBEpisodeID)
        {
            try
            {
                AniDB_Episode ep = Repo.Instance.AniDB_Episode.GetByEpisodeID(aniDBEpisodeID);

                if (ep == null) return "Could not find Episode";

                using (var upd = Repo.Instance.CrossRef_AniDB_Provider.BeginAddOrUpdate(() => Repo.Instance.CrossRef_AniDB_Provider.GetByAnimeIDAndType(ep.AnimeID, CrossRefType.TvDB).FirstOrDefault(a => a.EpisodesListOverride.GetByProviderId(tvDBEpisodeID.ToString()) != null)))
                {
                    if (!upd.IsUpdate)
                       return "Could not find Link!";
                    upd.Entity.EpisodesListOverride.DeleteFromProviderEpisodeId(tvDBEpisodeID.ToString());
                    if (upd.Entity.EpisodesListOverride.NeedPersitance)
                        upd.Commit();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return ex.Message;
            }
        }

        [HttpGet("TvDB/Poster/{tvDBID?}")]
        public List<TvDB_ImagePoster> GetAllTvDBPosters(int? tvDBID)
        {
            List<TvDB_ImagePoster> allImages = new List<TvDB_ImagePoster>();
            try
            {
                if (tvDBID.HasValue)
                    return Repo.Instance.TvDB_ImagePoster.GetBySeriesID(tvDBID.Value);
                else
                    return Repo.Instance.TvDB_ImagePoster.GetAll().ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return new List<TvDB_ImagePoster>();
            }
        }

        [HttpGet("TvDB/Banner/{tvDBID?}")]
        public List<TvDB_ImageWideBanner> GetAllTvDBWideBanners(int? tvDBID)
        {
            try
            {
                if (tvDBID.HasValue)
                    return Repo.Instance.TvDB_ImageWideBanner.GetBySeriesID(tvDBID.Value);
                else
                    return Repo.Instance.TvDB_ImageWideBanner.GetAll().ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return new List<TvDB_ImageWideBanner>();
            }
        }

        [HttpGet("TvDB/Fanart/{tvDBID?}")]
        public List<TvDB_ImageFanart> GetAllTvDBFanart(int? tvDBID)
        {
            try
            {
                if (tvDBID.HasValue)
                    return Repo.Instance.TvDB_ImageFanart.GetBySeriesID(tvDBID.Value);
                else
                    return Repo.Instance.TvDB_ImageFanart.GetAll().ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return new List<TvDB_ImageFanart>();
            }
        }

        [HttpGet("TvDB/Episode/{tvDBID?}")]
        public List<TvDB_Episode> GetAllTvDBEpisodes(int? tvDBID)
        {
            try
            {
                if (tvDBID.HasValue)
                    return Repo.Instance.TvDB_Episode.GetBySeriesID(tvDBID.Value);
                else
                    return Repo.Instance.TvDB_Episode.GetAll().ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return new List<TvDB_Episode>();
            }
        }

        #endregion

        #region Trakt

        [HttpGet("Trakt/Episode/{traktShowID?}")]
        public List<Trakt_Episode> GetAllTraktEpisodes(int? traktShowID)
        {
            try
            {
                if (traktShowID.HasValue)
                    return Repo.Instance.Trakt_Episode.GetByShowID(traktShowID.Value).ToList();
                else
                    return Repo.Instance.Trakt_Episode.GetAll().ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return new List<Trakt_Episode>();
            }
        }

        [HttpGet("Trakt/Episode/FromTraktId/{traktID}")]
        public List<Trakt_Episode> GetAllTraktEpisodesByTraktID(string traktID)
        {
            try
            {
                Trakt_Show show = Repo.Instance.Trakt_Show.GetByTraktSlug(traktID);
                if (show != null)
                    return GetAllTraktEpisodes(show.Trakt_ShowID);

                return new List<Trakt_Episode>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return new List<Trakt_Episode>();
            }
        }

        [HttpGet("WebCache/CrossRef/Trakt/{animeID}/{isAdmin}")]
        public List<WebCache_CrossRef_AniDB_Trakt> GetTraktCrossRefWebCache(int animeID, bool isAdmin)
        {
            try
            {
                if (isAdmin)
                    return WebCacheAPI.Admin_Get_CrossRefAniDBTrakt(animeID);
                else
                    return WebCacheAPI.Get_CrossRefAniDBTrakt(animeID);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return new List<WebCache_CrossRef_AniDB_Trakt>();
            }
        }

        [HttpPost("Trakt/CrossRef/{animeID}/{aniEpType}/{aniEpNumber}/{traktID}/{seasonNumber}/{traktEpNumber}/{crossRef_AniDB_TraktV2ID?}")]
        public string LinkAniDBTrakt(int animeID, int aniEpType, int aniEpNumber, string traktID, int seasonNumber,
            int traktEpNumber, int? crossRef_AniDB_TraktV2ID)
        {
            try
            {
                if (crossRef_AniDB_TraktV2ID.HasValue)
                {
                    CrossRef_AniDB_TraktV2 xrefTemp =
                        Repo.Instance.CrossRef_AniDB_TraktV2.GetByID(crossRef_AniDB_TraktV2ID.Value);
                    // delete the existing one if we are updating
                    TraktTVHelper.RemoveLinkAniDBTrakt(xrefTemp.AnimeID, (EpisodeType) xrefTemp.AniDBStartEpisodeType,
                        xrefTemp.AniDBStartEpisodeNumber,
                        xrefTemp.TraktID, xrefTemp.TraktSeasonNumber, xrefTemp.TraktStartEpisodeNumber);
                }

                CrossRef_AniDB_TraktV2 xref = Repo.Instance.CrossRef_AniDB_TraktV2.GetByTraktID(traktID, seasonNumber,
                    traktEpNumber, animeID,
                    aniEpType,
                    aniEpNumber);
                if (xref != null)
                {
                    string msg = string.Format("You have already linked Anime ID {0} to this Trakt show/season/ep",
                        xref.AnimeID);
                    SVR_AniDB_Anime anime = Repo.Instance.AniDB_Anime.GetByID(xref.AnimeID);
                    if (anime != null)
                    {
                        msg = string.Format("You have already linked Anime {0} ({1}) to this Trakt show/season/ep",
                            anime.MainTitle,
                            xref.AnimeID);
                    }
                    return msg;
                }

                return TraktTVHelper.LinkAniDBTrakt(animeID, (EpisodeType) aniEpType, aniEpNumber, traktID,
                    seasonNumber,
                    traktEpNumber, false);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return ex.Message;
            }
        }

        [HttpGet("Trakt/CrossRef/{animeID}")]
        public List<CrossRef_AniDB_TraktV2> GetTraktCrossRefV2(int animeID)
        {
            try
            {
                return Repo.Instance.CrossRef_AniDB_TraktV2.GetByAnimeID(animeID).Cast<CrossRef_AniDB_TraktV2>().ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return null;
            }
        }

        [HttpGet("Trakt/CrossRef/Episode/{animeID}")]
        public List<CrossRef_AniDB_Trakt_Episode> GetTraktCrossRefEpisode(int animeID)
        {
            try
            {
                return Repo.Instance.CrossRef_AniDB_Trakt_Episode.GetByAnimeID(animeID);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return null;
            }
        }

        [HttpGet("Trakt/Search/{criteria}")]
        public List<CL_TraktTVShowResponse> SearchTrakt(string criteria)
        {
            List<CL_TraktTVShowResponse> results = new List<CL_TraktTVShowResponse>();
            try
            {
                List<TraktV2SearchShowResult> traktResults = TraktTVHelper.SearchShowV2(criteria);

                foreach (TraktV2SearchShowResult res in traktResults)
                    results.Add(res.ToContract());

                return results;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return results;
            }
        }

        [HttpDelete("Trakt/CrossRef/{animeID}")]
        public string RemoveLinkAniDBTraktForAnime(int animeID)
        {
            try
            {
                SVR_AnimeSeries ser = Repo.Instance.AnimeSeries.GetByAnimeID(animeID);

                if (ser == null) return "Could not find Series for Anime!";

                // check if there are default images used associated
                List<AniDB_Anime_DefaultImage> images = Repo.Instance.AniDB_Anime_DefaultImage.GetByAnimeID(animeID);
                foreach (AniDB_Anime_DefaultImage image in images)
                {
                    if (image.ImageParentType == (int) ImageEntityType.Trakt_Fanart ||
                        image.ImageParentType == (int) ImageEntityType.Trakt_Poster)
                    {
                        Repo.Instance.AniDB_Anime_DefaultImage.Delete(image.AniDB_Anime_DefaultImageID);
                    }
                }

                foreach (CrossRef_AniDB_TraktV2 xref in Repo.Instance.CrossRef_AniDB_TraktV2.GetByAnimeID(animeID))
                {
                    TraktTVHelper.RemoveLinkAniDBTrakt(animeID, (EpisodeType) xref.AniDBStartEpisodeType,
                        xref.AniDBStartEpisodeNumber,
                        xref.TraktID, xref.TraktSeasonNumber, xref.TraktStartEpisodeNumber);
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return ex.Message;
            }
        }

        [HttpDelete("Trakt/CrossRef/{animeID}/{aniEpType}/{aniEpNumber}/{traktID}/{traktSeasonNumber}/{traktEpNumber}")]
        public string RemoveLinkAniDBTrakt(int animeID, int aniEpType, int aniEpNumber, string traktID,
            int traktSeasonNumber,
            int traktEpNumber)
        {
            try
            {
                SVR_AnimeSeries ser = Repo.Instance.AnimeSeries.GetByAnimeID(animeID);

                if (ser == null) return "Could not find Series for Anime!";

                // check if there are default images used associated
                List<AniDB_Anime_DefaultImage> images = Repo.Instance.AniDB_Anime_DefaultImage.GetByAnimeID(animeID);
                foreach (AniDB_Anime_DefaultImage image in images)
                {
                    if (image.ImageParentType == (int) ImageEntityType.Trakt_Fanart ||
                        image.ImageParentType == (int) ImageEntityType.Trakt_Poster)
                    {
                        Repo.Instance.AniDB_Anime_DefaultImage.Delete(image.AniDB_Anime_DefaultImageID);
                    }
                }

                TraktTVHelper.RemoveLinkAniDBTrakt(animeID, (EpisodeType) aniEpType, aniEpNumber,
                    traktID, traktSeasonNumber, traktEpNumber);

                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return ex.Message;
            }
        }

        [HttpGet("Trakt/Seasons/{traktID}")]
        public List<int> GetSeasonNumbersForTrakt(string traktID)
        {
            List<int> seasonNumbers = new List<int>();
            try
            {
                // refresh show info including season numbers from trakt
                TraktV2ShowExtended tvshow = TraktTVHelper.GetShowInfoV2(traktID);

                Trakt_Show show = Repo.Instance.Trakt_Show.GetByTraktSlug(traktID);
                if (show == null) return seasonNumbers;

                foreach (Trakt_Season season in show.GetSeasons())
                    seasonNumbers.Add(season.Season);

                return seasonNumbers;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return seasonNumbers;
            }
        }

        [HttpDelete("Trakt/Friend/{friendUsername}")]
        public CL_Response<bool> TraktFriendRequestDeny(string friendUsername)
        {
            return new CL_Response<bool> {Result = false};
            /*
            try
            {
                return TraktTVHelper.FriendRequestDeny(friendUsername, ref returnMessage);
            }
            catch (Exception ex)
            {
                logger.Error( ex,"Error in TraktFriendRequestDeny: " + ex.ToString());
                returnMessage = ex.Message;
                return false;
            }*/
        }

        [HttpPost("Trakt/Friend/{friendUsername}")]
        public CL_Response<bool> TraktFriendRequestApprove(string friendUsername)
        {
            return new CL_Response<bool> {Result = false};
            /*
            try
            {
                return TraktTVHelper.FriendRequestApprove(friendUsername, ref returnMessage);
            }
            catch (Exception ex)
            {
                logger.Error( ex,"Error in TraktFriendRequestDeny: " + ex.ToString());
                returnMessage = ex.Message;
                return false;
            }*/
        }

        [HttpPost("Trakt/Scrobble/{animeId}/{type}/{progress}/{status}")]
        public int TraktScrobble(int animeId, int type, int progress, int status)
        {
            try
            {
                Providers.TraktTV.ScrobblePlayingStatus statusTraktV2 = Providers.TraktTV.ScrobblePlayingStatus.Start;

                switch (status)
                {
                    case (int)Providers.TraktTV.ScrobblePlayingStatus.Start:
                        statusTraktV2 = Providers.TraktTV.ScrobblePlayingStatus.Start;
                        break;
                    case (int)Providers.TraktTV.ScrobblePlayingStatus.Pause:
                        statusTraktV2 = Providers.TraktTV.ScrobblePlayingStatus.Pause;
                        break;
                    case (int)Providers.TraktTV.ScrobblePlayingStatus.Stop:
                        statusTraktV2 = Providers.TraktTV.ScrobblePlayingStatus.Stop;
                        break;
                }

                bool isValidProgress = float.TryParse(progress.ToString(), out float progressTrakt);

                if (isValidProgress)
                {
                    switch (type)
                    {
                        // Movie
                        case (int) Providers.TraktTV.ScrobblePlayingType.movie:
                            return Providers.TraktTV.TraktTVHelper.Scrobble(
                                Providers.TraktTV.ScrobblePlayingType.movie, animeId.ToString(),
                                statusTraktV2, progressTrakt);
                        // TV episode
                        case (int) Providers.TraktTV.ScrobblePlayingType.episode:
                            return Providers.TraktTV.TraktTVHelper.Scrobble(
                                Providers.TraktTV.ScrobblePlayingType.episode,
                                animeId.ToString(), statusTraktV2, progressTrakt);
                    }
                }
                return 500;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return 500;
            }
        }

        [HttpPost("Trakt/Refresh/{traktID}")]
        public string UpdateTraktData(string traktD)
        {
            try
            {
                TraktTVHelper.UpdateAllInfo(traktD);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
            }
            return string.Empty;
        }

        [HttpPost("Trakt/Sync/{animeID}")]
        public string SyncTraktSeries(int animeID)
        {
            try
            {
                if (!ServerSettings.Instance.TraktTv.Enabled) return string.Empty;

                SVR_AnimeSeries ser = Repo.Instance.AnimeSeries.GetByAnimeID(animeID);
                if (ser == null) return "Could not find Anime Series";
                CommandQueue.Queue.Instance.Add(new CmdTraktSyncCollectionSeries(ser.AnimeSeriesID,ser.GetSeriesName()));


                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return ex.Message;
            }
        }

        [HttpPost("Trakt/Comment/{traktID}/{isSpoiler}")]
        public CL_Response<bool> PostTraktCommentShow(string traktID, string commentText, bool isSpoiler)
        {
            return TraktTVHelper.PostCommentShow(traktID, commentText, isSpoiler);
        }

        [HttpPost("Trakt/LinkValidity/{slug}/{removeDBEntries}")]
        public bool CheckTraktLinkValidity(string slug, bool removeDBEntries)
        {
            try
            {
                return TraktTVHelper.CheckTraktValidity(slug, removeDBEntries);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
            }
            return false;
        }

        [HttpGet("Trakt/CrossRef")]
        public List<CrossRef_AniDB_TraktV2> GetAllTraktCrossRefs()
        {
            try
            {
                return Repo.Instance.CrossRef_AniDB_TraktV2.GetAll().Cast<CrossRef_AniDB_TraktV2>().ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
            }
            return new List<CrossRef_AniDB_TraktV2>();
        }

        [HttpGet("Trakt/Comment/{animeID}")]
        public List<CL_Trakt_CommentUser> GetTraktCommentsForAnime(int animeID)
        {
            List<CL_Trakt_CommentUser> comments = new List<CL_Trakt_CommentUser>();

            try
            {
                List<TraktV2Comment> commentsTemp = TraktTVHelper.GetShowCommentsV2(animeID);
                if (commentsTemp == null || commentsTemp.Count == 0) return comments;

                foreach (TraktV2Comment sht in commentsTemp)
                {
                    CL_Trakt_CommentUser comment = new CL_Trakt_CommentUser();

                    Trakt_Friend traktFriend = Repo.Instance.Trakt_Friend.GetByUsername(sht.user.username);

                    // user details
                    comment.User = new CL_Trakt_User();
                    if (traktFriend == null)
                        comment.User.Trakt_FriendID = 0;
                    else
                        comment.User.Trakt_FriendID = traktFriend.Trakt_FriendID;

                    comment.User.Username = sht.user.username;
                    comment.User.Full_name = sht.user.name;

                    // comment details
                    comment.Comment = new CL_Trakt_Comment
                    {
                        CommentType = (int)TraktActivityType.Show, // episode or show
                        Text = sht.comment,
                        Spoiler = sht.spoiler,
                        Inserted = sht.CreatedAtDate,

                        // urls
                        Comment_Url = string.Format(TraktURIs.WebsiteComment, sht.id)
                    };
                    comments.Add(comment);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
            }
            return comments;
        }

        [HttpGet("Trakt/DeviceCode")]
        public CL_TraktDeviceCode GetTraktDeviceCode()
        {
            try
            {
                var response = TraktTVHelper.GetTraktDeviceCode();
                return new CL_TraktDeviceCode
                {
                    VerificationUrl = response.VerificationUrl,
                    UserCode = response.UserCode
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in GetTraktDeviceCode: " + ex.ToString());
                return null;
            }
        }

        #endregion

        #region Other Cross Refs

        [HttpGet("WebCache/CrossRef/Other/{animeID}/{crossRefType}")]
        public CL_CrossRef_AniDB_Provider_Response GetOtherAnimeCrossRefWebCache(int animeID, int crossRefType)
        {
            try
            {
                return WebCacheAPI.Get_CrossRefAniDBOther(animeID, (CrossRefType) crossRefType);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return null;
            }
        }

        [HttpGet("Other/CrossRef/{animeID}/{crossRefType}")]
        public CrossRef_AniDB_Other GetOtherAnimeCrossRef(int animeID, int crossRefType)
        {
            try
            {
                return Repo.Instance.CrossRef_AniDB_Other.GetByAnimeIDAndType(animeID, (CrossRefType) crossRefType);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return null;
            }
        }

        [HttpPost("Other/CrossRef/{animeID}/{id}/{crossRefType}")]
        public string LinkAniDBOther(int animeID, int movieID, int crossRefType)
        {
            try
            {
                CrossRefType xrefType = (CrossRefType) crossRefType;

                switch (xrefType)
                {
                    case CrossRefType.MovieDB:
                        MovieDBHelper.LinkAniDBMovieDB(animeID, movieID, false);
                        break;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return ex.Message;
            }
        }

        [HttpDelete("Other/CrossRef/{animeID}/{crossRefType}")]
        public string RemoveLinkAniDBOther(int animeID, int crossRefType)
        {
            try
            {
                SVR_AniDB_Anime anime = Repo.Instance.AniDB_Anime.GetByID(animeID);

                if (anime == null) return "Could not find Anime!";

                CrossRefType xrefType = (CrossRefType) crossRefType;
                switch (xrefType)
                {
                    case CrossRefType.MovieDB:

                        // check if there are default images used associated
                        List<AniDB_Anime_DefaultImage> images =
                            Repo.Instance.AniDB_Anime_DefaultImage.GetByAnimeID(animeID);
                        foreach (AniDB_Anime_DefaultImage image in images)
                        {
                            if (image.ImageParentType == (int) ImageEntityType.MovieDB_FanArt ||
                                image.ImageParentType == (int) ImageEntityType.MovieDB_Poster)
                            {
                                Repo.Instance.AniDB_Anime_DefaultImage.Delete(image.AniDB_Anime_DefaultImageID);
                            }
                        }
                        MovieDBHelper.RemoveLinkAniDBMovieDB(animeID);
                        break;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return ex.Message;
            }
        }

        #endregion

        #region MovieDB

        [HttpGet("MovieDB/Search/{criteria}")]
        public List<CL_MovieDBMovieSearch_Response> SearchTheMovieDB(string criteria)
        {
            List<CL_MovieDBMovieSearch_Response> results = new List<CL_MovieDBMovieSearch_Response>();
            try
            {
                List<MovieDB_Movie_Result> movieResults = MovieDBHelper.Search(criteria);

                foreach (MovieDB_Movie_Result res in movieResults)
                    results.Add(res.ToContract());

                return results;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return results;
            }
        }

        [HttpGet("MovieDB/Poster/{movieID?}")]
        public List<MovieDB_Poster> GetAllMovieDBPosters(int? movieID)
        {
            try
            {
                if (movieID.HasValue)
                    return Repo.Instance.MovieDB_Poster.GetByMovieID(movieID.Value);
                else
                    return Repo.Instance.MovieDB_Poster.GetAllOriginal();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return new List<MovieDB_Poster>();
            }
        }

        [HttpGet("MovieDB/Fanart/{movieID?}")]
        public List<MovieDB_Fanart> GetAllMovieDBFanart(int? movieID)
        {
            try
            {
                if (movieID.HasValue)
                    return Repo.Instance.MovieDB_Fanart.GetByMovieID(movieID.Value);
                else
                    return Repo.Instance.MovieDB_Fanart.GetAllOriginal();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
                return new List<MovieDB_Fanart>();
            }
        }

        [HttpPost("MovieDB/Refresh/{movieID}")]
        public string UpdateMovieDBData(int movieD)
        {
            try
            {
                MovieDBHelper.UpdateMovieInfo(movieD, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.ToString());
            }
            return string.Empty;
        }

        #endregion
    }
}
