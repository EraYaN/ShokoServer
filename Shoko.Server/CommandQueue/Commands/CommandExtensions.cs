﻿using System;
using System.Reflection;
using Shoko.Models.Server;


namespace Shoko.Server.CommandQueue.Commands
{
    public static class CommandExtensions
    {
        public static ICommand ToCommand(this CommandRequest cmd) 
        {
            Type t = Type.GetType(cmd.Class);
            if (t == null)
                return null;
            ConstructorInfo ctor = t.GetConstructor(new[] {typeof(string)});
            if (ctor == null)
                return null;
            ICommand worker = (ICommand) ctor.Invoke(new object[] {cmd.Data});
            worker.ParallelMax = cmd.ParallelMax;
            worker.ParallelTag = cmd.ParallelTag;
            worker.Priority = cmd.Priority;
            worker.Retries = cmd.Retries;
            worker.Batch = cmd.Batch;
            return worker;
        }

        public static CommandRequest ToCommandRequest(this ICommand b)
        {
            CommandRequest c = new CommandRequest();
            c.Id = b.Id;
            c.Class = b.GetType().ToString();
            c.Type = (int) b.WorkType;
            c.ParallelMax = b.ParallelMax;
            c.ParallelTag = b.ParallelTag;
            c.Priority = b.Priority;
            c.Retries = b.Retries;
            c.Batch = b.Batch;
            c.Data = b.Serialize();
            return c;
        }
    }
}