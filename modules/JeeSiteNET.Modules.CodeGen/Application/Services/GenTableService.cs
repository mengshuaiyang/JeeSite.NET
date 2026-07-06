using JeeSiteNET.Core;



using JeeSiteNET.Modules.CodeGen.Application.DTOs;



using JeeSiteNET.Modules.CodeGen.Domain.Entities;



using JeeSiteNET.Modules.CodeGen.Domain.Interfaces;



using Microsoft.EntityFrameworkCore;



namespace JeeSiteNET.Modules.CodeGen.Application.Services;

/// <summary>代码生成配置表领域服务，负责 GenTable / GenTableColumn 的分页查询、保存、级联删除等数据操作。</summary>

public class GenTableService



{



    private readonly IGenTableRepository _genTableRepository;



    private readonly IGenTableColumnRepository _genTableColumnRepository;



    public GenTableService(IGenTableRepository genTableRepository, IGenTableColumnRepository genTableColumnRepository)



    {



        _genTableRepository = genTableRepository;



        _genTableColumnRepository = genTableColumnRepository;



    }

    /// <summary>分页查询方法（按条件返回分页结果）。</summary>

    public async Task<PageResult<GenTableDto>> FindPageAsync(PageRequest<GenTable> request)



    {



        var query = _genTableRepository.Query()



            .WhereIf(!string.IsNullOrEmpty(request.Entity?.TableName), e => e.TableName.Contains(request.Entity!.TableName!))



            .WhereIf(!string.IsNullOrEmpty(request.Entity?.ClassName), e => e.ClassName.Contains(request.Entity!.ClassName!))



            .OrderBy(e => e.TableName);



        var total = await query.CountAsync();



        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();



        return new PageResult<GenTableDto> { List = list.Select(GenTableDto.FromEntity).ToList(), Total = total, PageNo = request.PageNo, PageSize = request.PageSize };



    }

    /// <summary>获取单条数据的异步方法（详情查询）。</summary>

    public async Task<GenTableDto?> GetAsync(string tableName)



    {



        var entity = await _genTableRepository.GetWithColumnsAsync(tableName);



        return entity == null ? null : GenTableDto.FromEntity(entity);



    }

    /// <summary>保存方法（新增或更新）。</summary>

    public async Task<ApiResult> SaveAsync(GenTableSaveDto dto)



    {



        var now = DateTime.Now;



        GenTable entity;



        if (!string.IsNullOrEmpty(dto.TableName) && await _genTableRepository.GetAsync(dto.TableName) != null)



        {



            entity = (await _genTableRepository.GetWithColumnsAsync(dto.TableName))!;



            entity.ClassName = dto.ClassName; entity.ModuleCode = dto.ModuleCode; entity.FunctionName = dto.FunctionName;



            entity.FunctionAuthor = dto.FunctionAuthor; entity.TableComment = dto.TableComment;



            entity.TplCategory = dto.TplCategory; entity.BusinessName = dto.BusinessName; entity.Status = dto.Status;



            entity.UpdateDate = now;



            foreach (var colDto in dto.Columns)



            {



                var col = entity.Columns.FirstOrDefault(c => c.ColumnName == colDto.ColumnName);



                if (col != null)



                {



                    col.NetType = colDto.NetType; col.PropertyName = colDto.PropertyName; col.ColumnSort = colDto.ColumnSort;



                    col.IsPk = colDto.IsPk; col.IsNullable = colDto.IsNullable;



                    col.IsInsert = colDto.IsInsert; col.IsEdit = colDto.IsEdit; col.IsList = colDto.IsList;



                    col.IsQuery = colDto.IsQuery; col.QueryType = colDto.QueryType; col.HtmlType = colDto.HtmlType;



                }



            }



            await _genTableRepository.UpdateAsync(entity);



        }



        else



        {



            entity = new GenTable



            {



                TableName = dto.TableName, ClassName = dto.ClassName, ModuleCode = dto.ModuleCode,



                FunctionName = dto.FunctionName, FunctionAuthor = dto.FunctionAuthor,



                TableComment = dto.TableComment, TplCategory = dto.TplCategory ?? "crud",



                BusinessName = dto.BusinessName, Status = dto.Status ?? "0",



                CreateDate = now, UpdateDate = now



            };



            int sort = 10;



            foreach (var colDto in dto.Columns)



            {



                entity.Columns.Add(new GenTableColumn



                {



                    ColumnId = $"{dto.TableName}.{colDto.ColumnName}",



                    TableName = dto.TableName, ColumnName = colDto.ColumnName,



                    ColumnComment = colDto.ColumnComment, NetType = colDto.NetType ?? "string",



                    PropertyName = colDto.PropertyName, ColumnSort = sort,



                    IsPk = colDto.IsPk ?? "0", IsNullable = colDto.IsNullable ?? "1",



                    IsInsert = colDto.IsInsert ?? "1", IsEdit = colDto.IsEdit ?? "1",



                    IsList = colDto.IsList ?? "1", IsQuery = colDto.IsQuery ?? "0",



                    QueryType = colDto.QueryType ?? "EQ", HtmlType = colDto.HtmlType ?? "input",



                    CreateDate = now, UpdateDate = now



                });



                sort += 10;



            }



            await _genTableRepository.AddAsync(entity);



        }



        return ApiResult.Ok(GenTableDto.FromEntity(entity));



    }

    /// <summary>删除方法。</summary>

    public async Task<ApiResult> DeleteAsync(string tableName)



    {



        var entity = await _genTableRepository.GetAsync(tableName);



        if (entity == null) return ApiResult.NotFound("配置不存在");



        var columns = await _genTableColumnRepository.FindByTableNameAsync(tableName);



        foreach (var col in columns)



            await _genTableColumnRepository.RemoveAsync(col);

        // 循环结束后统一一次提交，避免逐列 SaveChanges 的性能开销
        await _genTableColumnRepository.SaveChangesAsync();



        await _genTableRepository.DeleteAsync(entity);



        return ApiResult.Ok();



    }



}

