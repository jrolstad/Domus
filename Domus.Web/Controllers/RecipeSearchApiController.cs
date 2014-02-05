﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domus.Web.Models.Api;

namespace Domus.Web.Controllers
{
    public class RecipeSearchApiController : ApiController
    {
        private RecipeApiController _apiController = new RecipeApiController();

        public IEnumerable<RecipeApiModel> Get()
        {
            return _apiController.Get();
        }

        public IEnumerable<RecipeApiModel> Get(RecipeSearchRequest request)
        {
            if (request == null)
                return new List<RecipeApiModel>();

            if (!string.IsNullOrWhiteSpace(request.Category))
                return _apiController
                    .Get()
                    .Where(r => r.Category == request.Category);
            
            if(!string.IsNullOrWhiteSpace(request.SearchTerms))
                return _apiController
                    .Get()
                    .Where(r => r.Name != null)
                    .Where(r => r.Name.Contains(request.SearchTerms));

            return new List<RecipeApiModel>();
        }

        
    }
}