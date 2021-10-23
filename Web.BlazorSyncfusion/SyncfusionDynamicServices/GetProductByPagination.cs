using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System.Resources;
using Application.MediatorHandlers;
using Khalifa.Framework;
using Syncfusion.Blazor;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading;
using Syncfusion.Blazor.Data;
using Application.MediatorHandlers.ProductHandlers;

namespace Web.BlazorSyncfusion.SyncfusionDynamicServices
{
    public class GetProductBySfComponent : DataAdaptor
    {
        private readonly IMediator _signal;

        public GetProductBySfComponent(IMediator signal)
        {
            _signal = signal;
        }

        public async override Task<object> ReadAsync(DataManagerRequest dataManagerRequest, string key = null)
        {

            var filterUi = ProductFilterUI.Default();
            var filter = filterUi.GetFilter();
            var sorter = ProductSorter.ByCreateDateDesc();

            var response = await _signal.Send(new GetproductsByAdabtor.Request(dataManagerRequest.Skip, dataManagerRequest.Take));

            if (!response.IsOK)
            {
                throw new ArgumentNullException();

            }

            DataResult dataResult = new DataResult()
            {
                Result = response.Products,
                Count = response.TotalItem
            };


            return dataResult;
        }
    }
}
