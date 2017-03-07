using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace MVCtutorial.Report.Models
{
    [Flags]
    public enum BatchStatus
    {
        None = 0,
        Amount = 1,
        Temperature = 2,
        StepTime = 4,
        InterStepTime = 8
    }

    [Flags]
    public enum StepStatus
    {
        None = 0,
        Forced = 1,
        OK = 2,
     }
    public class ReportViewModel
    {
        public Batch[] Batches; //array of current batches 
    }
    public class Batch
    {
        public uint Id /*diRecordNo directly from db*/ { get; set; }
        public DateTime StartTime /*In pkTime from server, at a client conversion to dateTime*/ { get; set; }
        public DateTime EndTime /*In pkTime from server, at a client conversion to dateTime*/ { get; set; }
        public short RecipeNr /*iRecipeNo directly from db*/ { get; set; }
        public string RecipeName /*string directly from db*/ { get; set; }
        public BatchStatus status /* flag type */ { get; set; }
        public List<RecipeStep> Steps /*List of Bataches for this Product */{ get; set; } //null until batch detail click
    }

    public class RecipeStep
    {
        //Whole model sorted from time ascending
        public DateTime StartTime /*In pkTime from server, at a client conversion to dateTime*/ { get; set; }
        public DateTime EndTime /*In pkTime from server, at a client conversion to dateTime*/ { get; set; }
        public byte OperationNr /*Resolve how to get that*/ { get; set; }
        public string Device /*DeviceName directly from db*/ { get; set; }
        public Int32 Need /*DeviceName directly from db*/ { get; set; }
        public Int32 Done /*diNeedDone directly from db*/ { get; set; }

        //Diff should be calculted by client

        public StepStatus Status /*siStatus directly from db*/ { get; set; }
        
    }
}