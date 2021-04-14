﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsFacade;
using EventsFacade.Events;
using OperationContracts.Enums;
using QueryService.Dto;

namespace QueryService.Services.Implementations
{
    internal class DocumentsToAnalysisService : IDocumentsToAnalysisService
    {
        private readonly AnalysisFacade _facade;

        public DocumentsToAnalysisService(AnalysisFacade facade)
        {
            _facade = facade;
        }

        public async Task<IEnumerable<DocumentToAnalysisResponse>> GetDocumentsToAnalysis(Guid userId)
        {
            var allAnalysisEvents = await _facade.GetAllUserDocumentAnalysesAsync(userId);

            var documents = allAnalysisEvents
                .Where(e => e.Status == OperationStatus.NotStarted);

            return documents.Select(e => new DocumentToAnalysisResponse(e.DocumentId,
                e.DocumentName,
                e.OccurenceDate));
        }

        public async Task<IEnumerable<AnalysisStatusDto>> GetDocumentsWithLatestAnalysisStatuses(
            Guid userId)
        {
            var allAnalysisEvents = await _facade.GetAllUserDocumentAnalysesAsync(userId);

            DocumentAnalysisStatusChangedEvent GetInitialEvent(Guid docId) =>
                allAnalysisEvents.First(e => e.Status == OperationStatus.NotStarted && e.DocumentId == docId);

            var filesWithCorrespondingAnalysesEvents = allAnalysisEvents
                .GroupBy(e => e.DocumentId)
                .ToList();

            if (!filesWithCorrespondingAnalysesEvents.Any())
            {
                return new List<AnalysisStatusDto>();
            }

            return filesWithCorrespondingAnalysesEvents.Select(f => f.OrderByDescending(e => e.OccurenceDate).First())
                .Select(e => new AnalysisStatusDto(e))
                .Select(e => e with { DocumentName = GetInitialEvent(e.DocumentId).DocumentName });
        }
    }
}