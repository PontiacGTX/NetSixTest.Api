using MediatR;
using NetSixTest.Data.Entity;
using NetSixTest.DataAccess.Command;
using NetSixTest.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.Services.Services
{
    public class ProductPictureServices(IMediator _mediator)
    {
        public async Task<IList<ProductPicture>> AddProductPicture(IList<ProductPicture> pictures)
        {
            List<ProductPicture> output = new List<ProductPicture>();
            foreach (ProductPicture picture in pictures)
            {
                try
                {
                    picture.Hash = FileHelper.ComputeImageHash(picture.PictueData);
                    output.Add(await _mediator.Send(new AddProductPictureCommand() { Field = picture }));
                }
                catch (Exception ex)
                {

                }

            }
            return output;
        }

       
    }
}
