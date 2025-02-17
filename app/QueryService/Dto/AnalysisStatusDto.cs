﻿using System;
using EventsFacade.Events;
using OperationContracts.Enums;

namespace QueryService.Dto
{
    public record AnalysisStatusDto
    {
        public AnalysisStatusDto(DocumentAnalysisStatusChangedEvent @event)
        {
            TaskId = @event.TaskId;
            DocumentId = @event.DocumentId;
            DocumentName = @event.DocumentName;
            LatestChangeDate = @event.OccurenceDate;
            Status = @event.Status;
            Result = @event.Result;
        }

        public string DocumentName { get; init; }
        public Guid DocumentId { get; init; }
        public Guid TaskId { get; init; }
        public OperationStatus Status { get; init; }
        public DateTime LatestChangeDate { get; init; }
        public double Result { get; init; }
    }
}