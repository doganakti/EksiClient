using System;
using System.Collections.Generic;

namespace EksiClient
{
    /// <summary>
    /// Result.
    /// </summary>
    public class Result<T>
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public Status Status { get; set; } = Status.Success;

        /// <summary>
        /// Gets the status description.
        /// </summary>
        /// <value>The status description.</value>
        public string StatusDescription => Status.ToString().SplitCamelCase();

        /// <summary>
        /// Gets or sets the result list.
        /// </summary>
        /// <value>The result list.</value>
        public List<T> ResultList { get; set; } = new List<T>();

        /// <summary>
        /// Gets or sets the pager.
        /// </summary>
        /// <value>The pager.</value>
        public Pager Pager { get; set; }

        /// <summary>
        /// Gets or sets the topic.
        /// </summary>
        /// <value>The topic.</value>
        public Topic Topic { get; set; }
    }
}
