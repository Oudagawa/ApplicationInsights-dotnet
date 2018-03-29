﻿namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>@ToDo: Complete documentation before stable release.</summary>
    /// @PublicExposureCandidate
    internal sealed class MetricAggregateToTelemetryPipelineConverters 
    {
        /// <summary>@ToDo: Complete documentation before stable release.</summary>
        public static readonly MetricAggregateToTelemetryPipelineConverters Registry = new MetricAggregateToTelemetryPipelineConverters();

        private ConcurrentDictionary<Type, ConcurrentDictionary<string, IMetricAggregateToTelemetryPipelineConverter>> pipelineTable
                                                        = new ConcurrentDictionary<Type, ConcurrentDictionary<string, IMetricAggregateToTelemetryPipelineConverter>>();

        /// <summary>@ToDo: Complete documentation before stable release.</summary>
        /// <param name="pipelineType">@ToDo: Complete documentation before stable release.</param>
        /// <param name="aggregationKindMoniker">@ToDo: Complete documentation before stable release.</param>
        /// <param name="converter">@ToDo: Complete documentation before stable release.</param>
        public void Add(Type pipelineType, string aggregationKindMoniker, IMetricAggregateToTelemetryPipelineConverter converter)
        {
            ValidateKeys(pipelineType, aggregationKindMoniker);
            Util.ValidateNotNull(converter, nameof(converter));

            ConcurrentDictionary<string, IMetricAggregateToTelemetryPipelineConverter> converters = this.pipelineTable.GetOrAdd(
                                                                                pipelineType,
                                                                                new ConcurrentDictionary<string, IMetricAggregateToTelemetryPipelineConverter>());

            converters[aggregationKindMoniker] = converter;
        }

        /// <summary>@ToDo: Complete documentation before stable release.</summary>
        /// <param name="pipelineType">@ToDo: Complete documentation before stable release.</param>
        /// <param name="aggregationKindMoniker">@ToDo: Complete documentation before stable release.</param>
        /// <param name="converter">@ToDo: Complete documentation before stable release.</param>
        /// <returns>@ToDo: Complete documentation before stable release.</returns>
        public bool TryGet(Type pipelineType, string aggregationKindMoniker, out IMetricAggregateToTelemetryPipelineConverter converter)
        {
            ValidateKeys(pipelineType, aggregationKindMoniker);

            ConcurrentDictionary<string, IMetricAggregateToTelemetryPipelineConverter> converters;
            if (false == this.pipelineTable.TryGetValue(pipelineType, out converters))
            {
                converter = null;
                return false;
            }

            bool hasConverter = converters.TryGetValue(aggregationKindMoniker, out converter);
            return hasConverter;
        }

        private static void ValidateKeys(Type pipelineType, string aggregationKindMoniker)
        {
            Util.ValidateNotNull(pipelineType, nameof(pipelineType));

            ////if (false == typeof(IMetricTelemetryPipeline).IsAssignableFrom(pipelineType))
            ////{
            ////    throw new ArgumentException($"{nameof(pipelineType)} must specify a type that implements the interface '{typeof(IMetricTelemetryPipeline).Name}'"
            ////                              + $", but it specifies the type '{pipelineType.Name}' that does not implement that interface.");
            ////}

            Util.ValidateNotNullOrWhitespace(aggregationKindMoniker, nameof(aggregationKindMoniker));
        }
    }
}
