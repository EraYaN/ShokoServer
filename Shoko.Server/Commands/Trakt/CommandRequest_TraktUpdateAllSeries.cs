﻿using System;
using System.Xml;
using Shoko.Commons.Queue;
using Shoko.Models.Queue;
using Shoko.Models.Server;
using Shoko.Server.Providers.TraktTV;
using Shoko.Server.Repositories;

namespace Shoko.Server.Commands
{
    [Serializable]
    [Command(CommandRequestType.Trakt_UpdateAllSeries)]
    public class CommandRequest_TraktUpdateAllSeries : CommandRequestImplementation
    {
        public bool ForceRefresh { get; set; }


        public override CommandRequestPriority DefaultPriority => CommandRequestPriority.Priority6;

        public override QueueStateStruct PrettyDescription => new QueueStateStruct {queueState = QueueStateEnum.UpdateTrakt, extraParams = new string[0]};

        public CommandRequest_TraktUpdateAllSeries()
        {
        }

        public CommandRequest_TraktUpdateAllSeries(bool forced)
        {
            Priority = (int) DefaultPriority;

            GenerateCommandID();
        }

        public override void ProcessCommand()
        {
            logger.Info("Processing CommandRequest_TraktUpdateAllSeries");

            try
            {
                using (var txn = Repo.ScheduledUpdate.BeginAddOrUpdate(
                    () => Repo.ScheduledUpdate.GetByUpdateType((int)ScheduledUpdateType.TraktUpdate),
                    () => new ScheduledUpdate { UpdateType = (int)ScheduledUpdateType.TraktUpdate, UpdateDetails = string.Empty }
                    ))
                {
                    
                    if (!txn.IsUpdate)
                    {
                        int freqHours = Utils.GetScheduledHours(ServerSettings.Instance.Trakt_UpdateFrequency);

                        // if we have run this in the last xxx hours then exit
                        TimeSpan tsLastRun = DateTime.Now - txn.Entity.LastUpdate;
                        if (tsLastRun.TotalHours < freqHours && !ForceRefresh)
                            return;
                    }
                    txn.Entity.LastUpdate = DateTime.Now;
                    txn.Commit();
                }

                // update all info
                TraktTVHelper.UpdateAllInfo();

                // scan for new matches
                TraktTVHelper.ScanForMatches();
            }
            catch (Exception ex)
            {
                logger.Error("Error processing CommandRequest_TraktUpdateAllSeries: {0}", ex);
            }
        }

        /// <summary>
        /// This should generate a unique key for a command
        /// It will be used to check whether the command has already been queued before adding it
        /// </summary>
        public override void GenerateCommandID()
        {
            CommandID = "CommandRequest_TraktUpdateAllSeries";
        }

        public override bool LoadFromDBCommand(CommandRequest cq)
        {
            CommandID = cq.CommandID;
            CommandRequestID = cq.CommandRequestID;
            Priority = cq.Priority;
            CommandDetails = cq.CommandDetails;
            DateTimeUpdated = cq.DateTimeUpdated;

            // read xml to get parameters
            if (CommandDetails.Trim().Length > 0)
            {
                XmlDocument docCreator = new XmlDocument();
                docCreator.LoadXml(CommandDetails);

                // populate the fields
                ForceRefresh =
                    bool.Parse(TryGetProperty(docCreator, "CommandRequest_TraktUpdateAllSeries", "ForceRefresh"));
            }

            return true;
        }

        public override CommandRequest ToDatabaseObject()
        {
            GenerateCommandID();

            CommandRequest cq = new CommandRequest
            {
                CommandID = CommandID,
                CommandType = CommandType,
                Priority = Priority,
                CommandDetails = ToXML(),
                DateTimeUpdated = DateTime.Now
            };
            return cq;
        }
    }
}