using System.Collections;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SystemTech.Core.Exceptions;
using SystemTech.Core.Utils;

namespace SystemTech.Core.Messages
{
    public class Merger : Mapper
    {
        public Merger(IConfigurationProvider configurationProvider) : base(configurationProvider)
        {
        }

        public Merger(IConfigurationProvider configurationProvider, Func<Type, object> serviceCtor) : base(configurationProvider, serviceCtor)
        {
        }

        public BaseResponse<List<TResult>, TField> MergeData<TResult, TField, TModel>(BaseResponse<List<TResult>, TField> destination, IQueryable<TModel> queryableModels) where TField : QueryFields
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
        
            if (queryableModels == null)
            {
                destination.AddException(new RecordNotFoundException(nameof(queryableModels)));
                return destination;
            }
        
            List<TModel> models = queryableModels.Skip(destination.Fields.Offset).Take(destination.Fields.Limit).ToList();
            List<TResult> tResults = Map<List<TResult>>(models);
            
            destination.SetResult(tResults);
            destination.SetTotal(queryableModels.Count());
        
            return destination;
        }
        
        public async Task<BaseResponse<List<TResult>, TField>> MergeDataAsync<TResult, TField, TModel>(BaseResponse<List<TResult>, TField> destination, IQueryable<TModel> queryableModels) where TField : QueryFields
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
        
            if (queryableModels == null)
            {
                destination.AddException(new RecordNotFoundException(nameof(queryableModels)));
                return destination;
            }
        
            List<TModel> models = await queryableModels.Skip(destination.Fields.Offset).Take(destination.Fields.Limit).ToListAsync();
            List<TResult> tResults = Map<List<TResult>>(models);
            
            destination.SetResult(tResults);
            destination.SetTotal(queryableModels.Count());
        
            return destination;
        }
        
        public BaseResponse<TResult, TField> MergeData<TResult, TField, TModel>(BaseResponse<TResult, TField> destination, List<TModel> model) where TResult : class, new() where TModel : new() where TField : class
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
        
            if (model == null)
            {
                destination.AddException(new RecordNotFoundException(nameof(model)));
                return destination;
            }
        
            TResult tResult = Map<TResult>(model);
            destination.SetResult(tResult);
            destination.SetTotal(model.Count);
        
            return destination;
        }
        
        public BaseResponse<TResult, TField> MergeData<TResult, TField, TModel>(BaseResponse<TResult, TField> destination, TModel model) where TResult : class, new() where TModel : new() where TField : class
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
        
            if (model == null)
            {
                destination.AddException(new RecordNotFoundException($"{typeof(TModel).Name} not found"));
                return destination;
            }
        
            TResult tResult = Map<TResult>(model);
            destination.SetResult(tResult);
            destination.SetTotal(1);
        
            return destination;
        }
        
        public BaseResponse<List<TResult>, TField> MergeData<TResult, TField, TModel>(BaseResponse<List<TResult>, TField> destination, List<TModel> models)
            where TResult : MessageModel, new()
            where TField : class, new()
            where TModel : MessageModel, new()
        {
            if (destination == null || models == null)
                throw new ArgumentNullException(nameof(destination));

            List<TResult> tResult = Map<List<TResult>>(models);
            destination.SetResult(tResult);
            destination.SetTotal(tResult.Count);

            return destination;
        }
        
        public BaseResponse<List<TResult>, List<TField>> MergeData<TResult, TField, TModel>(BaseResponse<List<TResult>, List<TField>> destination, List<TModel> models)
            where TResult : MessageModel, new()
            where TField : MessageModel, new()
            where TModel : MessageModel, new()
        {
            if (destination == null || models == null)
                throw new ArgumentNullException(nameof(destination));

            foreach (TField destinationField in destination.Fields)
            {
                TModel model = models.FirstOrDefault(item => item.BusinessKey == destinationField.BusinessKey);
                var keyProperties = ReflectionHelpers.GetKeyAttributes<TField>();
                var keys = keyProperties.Select(keyProperty => keyProperty.GetValue(destinationField, null)?.ToString()).ToArray();
                if (model == null)
                {
                    // TODO: plz check carefully
                    if (typeof(TModel) == typeof(TField))
                        model = Map<TModel>(destinationField);
                    else
                        model = new TModel();
                    model.Exception = new RecordNotFoundException($"{typeof(TField).Name}: {string.Join("_", keys)} was not found!");
                    models.Add(model);
                }
            }
            
            List<TResult> tResult = Map<List<TResult>>(models);
            destination.SetResult(tResult);
            destination.SetTotal(tResult.Count);
            destination.AddException(tResult.Where(result => result.Exception != null).Select(result => result.Exception).ToList());

            return destination;
        }
        
        public void Merge<TField>(BaseRequest<TField> source, TField tFields) where TField : MessageModel
        {
            source.SetFields(tFields);
            if (tFields.Exception != null)
            {
                source.AddException(tFields.Exception);
            }
        }
        
        public void Merge<TField>(BaseRequest<List<TField>> source, List<TField> tFields) where TField : MessageModel
        {
            source.SetFields(tFields);
            if (tFields.Any(item => item.Exception != null))
            {
                source.AddException(tFields.Where(item => item.Exception != null).Select(item => item.Exception).ToList());
            }
        }
        
        
        public void Merge<TResultSource, TFieldSource, TResultDest, TFieldDest>(BaseResponse<TResultDest, TFieldDest> destination, BaseResponse<TResultSource, TFieldSource> source)
            where TResultDest : class, new()
            where TFieldDest : class, new()
            where TFieldSource : class
            where TResultSource : class, new()
        {
            TResultDest resultDest = MergeProperty<TResultDest, TResultSource>(source.Result);
            TFieldDest fieldDest = MergeProperty<TFieldDest, TFieldSource>(source.Fields);
            destination.SetTotal(source.Total);
            destination.SetResult(resultDest);
            destination.SetFields(fieldDest);
            destination.AddException(source.ErrorMessages);
        }

        private bool isGenericList<T>(T t) => t is IList && typeof(T).IsGenericType;
        
        private TDestination MergeProperty<TDestination, TSource>(TSource source) where TDestination : new()
        {
            TDestination destination = new TDestination();
            
            if (isGenericList<TDestination>(destination) && !isGenericList<TSource>(source))
            {
                List<TDestination> fieldSourceList = Map<List<TDestination>>(source);
                destination = fieldSourceList.First();
            }
            else if (!isGenericList<TDestination>(destination) && isGenericList<TSource>(source))
            {
                List<TDestination> fieldDestList = Map<List<TDestination>>(source);
                destination = fieldDestList.FirstOrDefault();
            }
            else
            {
                destination = Map<TDestination>(source);
            }

            return destination;
        }
    }
}