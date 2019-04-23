// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-15
// Last Modified:			2019-04-23
//

namespace cloudscribe.Web.Pagination
{

    public class PaginationSettings
    {
        public long TotalItems { get; set; } = 0;

        public int ItemsPerPage { get; set; } = 10;

        public long CurrentPage { get; set; } = 1;

        public long MaxPagerItems { get; set; } = 10;

        public bool ShowFirstLast { get; set; } = false;

        public bool ShowNumbered { get; set; } = true;

        public bool UseReverseIncrement { get; set; } = false;

        public bool SuppressEmptyNextPrev { get; set; } = false;

        public bool SuppressInActiveFirstLast { get; set; } = false;

        public bool RemoveNextPrevLinks { get; set; } = false;

    }

}
