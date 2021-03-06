using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PlainElastic.Net.Utils;

namespace PlainElastic.Net.Queries
{
	public abstract class HistogramAggregationBase<TAggregation, T> : AggregationBase<TAggregation, T> where TAggregation : HistogramAggregationBase<TAggregation, T>
    {
        /// <summary>
		/// The field to execute aggregation against.
        /// </summary>
		public TAggregation Field(string fieldName)
        {
            RegisterJsonPart("'field': {0}", fieldName.Quotate());
			return (TAggregation)this;
        }

        /// <summary>
        /// The field to execute aggregation against.
        /// </summary>
		public TAggregation Field(Expression<Func<T, object>> field)
        {
            return Field(field.GetPropertyPath());
        }

        /// <summary>
		/// The field to execute aggregation against.
        /// </summary>
		public TAggregation FieldOfCollection<TProp>(Expression<Func<T, IEnumerable<TProp>>> collectionField, Expression<Func<TProp, object>> field)
        {
            var collectionProperty = collectionField.GetPropertyPath();
            var fieldName = collectionProperty + "." + field.GetPropertyPath();

            return Field(fieldName);
        }

		/// <summary>
		/// It is possible to only return buckets that have a document count that is greater than or equal to a configured limit through the min_doc_count option.
		/// </summary>
		public TAggregation MinDocCount(long minDocCount)
		{
			RegisterJsonPart("'min_doc_count': {0}", minDocCount.AsString());
			return (TAggregation)this;
		}

		/// <summary>
		/// With extended_bounds setting, you now can "force" the histogram aggregation to start building buckets on a specific min values and also keep on building buckets up to a max value (even if there are no documents anymore). Using extended_bounds only makes sense when min_doc_count is 0 (the empty buckets will never be returned if min_doc_count is greater than 0).
		/// </summary>
		public TAggregation ExtendedBounds(long min, long max)
		{
			RegisterJsonPart("'extended_bounds': {{ 'min': {0}, 'max': {1} }}", min.AsString(), max.AsString());
			return (TAggregation)this;
		}

		/// <summary>
		/// With extended_bounds setting, you now can "force" the histogram aggregation to start building buckets on a specific min values and also keep on building buckets up to a max value (even if there are no documents anymore). Using extended_bounds only makes sense when min_doc_count is 0 (the empty buckets will never be returned if min_doc_count is greater than 0).
		/// </summary>
		public TAggregation Order(string key, string direction)
		{
			RegisterJsonPart("'order': {{ {0}: {1} }}", key.Quotate(), direction.Quotate());
			return (TAggregation)this;
		}

    }
}