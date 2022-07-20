using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.DTOs;
using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.WebApiModels;

namespace Quid_pro_Quo.Mappings
{
    public static class PostMapping
    {
       public static PostFormDTO ToPostFormDTO(this PostFormApiModel model, string authorName, DateTime postedAt)
            => new PostFormDTO()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Text = model.Text,
                    ImageFiles = model.ImageFiles,
                    AuthorName = authorName,
                    PostedAt = postedAt,
                    IsActual = true,
                    PerformServiceOnDatesList = model.PerformServiceOnDatesList,
                    PerformServiceInPlaceLat = model.PerformServiceInPlaceLat,
                    PerformServiceInPlaceLng = model.PerformServiceInPlaceLng,
                    PerformServiceInPlaceZoom = model.PerformServiceInPlaceZoom,
                };


        public static async Task<PostEntity> ToPostEntity(this PostFormDTO model, IUserRepository userRepository, IEnumerable<string> fileNames)
            => new PostEntity()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Text = model.Text,
                    ImageFileNames = string.Join(";", fileNames),
                    AuthorId = (await userRepository.GetByName(model.AuthorName)).Id,
                    PostedAt = model.PostedAt,
                    IsActual = true,
                    PerformServiceOnDatesList = String.Join(";", model.PerformServiceOnDatesList.Select(e => e.ToString())),
                    PerformServiceInPlaceLat = model.PerformServiceInPlaceLat,
                    PerformServiceInPlaceLng = model.PerformServiceInPlaceLng,
                    PerformServiceInPlaceZoom = model.PerformServiceInPlaceZoom,
                };

        public static async Task<PostGetApiModel> ToPostGetApiModel(this PostEntity entity, IUserRepository userRepository)
           => new PostGetApiModel()
               {
                   Id = entity.Id,
                   Title = entity.Title,
                   Text = entity.Text,
                   ImageFileNames = entity.ImageFileNames,
                   AuthorName = (await userRepository.GetById(entity.AuthorId)).UserName,
                   PostedAt = entity.PostedAt,
                   IsActual = entity.IsActual,
                   PerformServiceOnDatesList =
                       string.IsNullOrEmpty(entity.PerformServiceOnDatesList)
                       ?
                       new DateTime[] {}
                       :
                       entity.PerformServiceOnDatesList.Split(";").Select(s => DateTime.Parse(s)),
                    PerformServiceInPlaceLat = entity.PerformServiceInPlaceLat,
                    PerformServiceInPlaceLng = entity.PerformServiceInPlaceLng,
                    PerformServiceInPlaceZoom = entity.PerformServiceInPlaceZoom,
               };
    }
}
