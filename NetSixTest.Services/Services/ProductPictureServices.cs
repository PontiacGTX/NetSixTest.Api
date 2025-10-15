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
        public async Task<IList<ProductPicture>> AddProductPicture(IList<InsertProductPictureModel> pictures)
        {
            List<ProductPicture> output = new List<ProductPicture>();
            foreach (InsertProductPictureModel picture in pictures)
            {
                try
                {
                    var pic = new ProductPicture();
                    pic.ProductId =picture.ProductId;
                    pic.PictureData = Convert.FromBase64String(picture.PictureData);
                    pic.Hash = FileHelper.ComputeImageHash(pic.PictureData);
                    pic.FileName = picture.FileName;

                    pic = await _mediator.Send(new AddProductPictureCommand() { Field = pic });
                    pic.PictureData = null;
                    output.Add(pic);
                }
                catch (Exception ex)
                {

                }

            }
            return output;
        }

       
    }
}
