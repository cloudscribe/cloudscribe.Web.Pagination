// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-09-27
// Last Modified:			2018-06-16
//

using System.Collections.Generic;

namespace cloudscribe.Pagination.Models
{
    public class PagedResult<T> where T : class
    {
        public PagedResult()
        {
            Data = new List<T>();
        }
        public List<T> Data { get; set; }
        public long TotalItems { get; set; } = 0;
        public long PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 1;
    }
}
