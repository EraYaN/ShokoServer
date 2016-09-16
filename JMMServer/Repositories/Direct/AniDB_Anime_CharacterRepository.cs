﻿using System.Collections.Generic;
using JMMServer.Entities;
using JMMServer.Repositories.NHibernate;
using NHibernate.Criterion;

namespace JMMServer.Repositories.Direct
{
    public class AniDB_Anime_CharacterRepository : BaseDirectRepository<AniDB_Anime_Character, int>
    {


        public List<AniDB_Anime_Character> GetByAnimeID(int id)
        {
            using (var session = JMMService.SessionFactory.OpenSession())
            {
                var cats = session
                    .CreateCriteria(typeof(AniDB_Anime_Character))
                    .Add(Restrictions.Eq("AnimeID", id))
                    .List<AniDB_Anime_Character>();

                return new List<AniDB_Anime_Character>(cats);
            }
        }

        public List<AniDB_Anime_Character> GetByAnimeID(ISessionWrapper session, int id)
        {
            var cats = session
                .CreateCriteria(typeof(AniDB_Anime_Character))
                .Add(Restrictions.Eq("AnimeID", id))
                .List<AniDB_Anime_Character>();

            return new List<AniDB_Anime_Character>(cats);
        }

        public List<AniDB_Anime_Character> GetByCharID(int id)
        {
            using (var session = JMMService.SessionFactory.OpenSession())
            {
                var cats = session
                    .CreateCriteria(typeof(AniDB_Anime_Character))
                    .Add(Restrictions.Eq("CharID", id))
                    .List<AniDB_Anime_Character>();

                return new List<AniDB_Anime_Character>(cats);
            }
        }

        public AniDB_Anime_Character GetByAnimeIDAndCharID(int animeid, int charid)
        {
            using (var session = JMMService.SessionFactory.OpenSession())
            {
                AniDB_Anime_Character cr = session
                    .CreateCriteria(typeof(AniDB_Anime_Character))
                    .Add(Restrictions.Eq("AnimeID", animeid))
                    .Add(Restrictions.Eq("CharID", charid))
                    .UniqueResult<AniDB_Anime_Character>();

                return cr;
            }
        }

    }
}