namespace JeeSiteNET.Modules.CodeGen.Application.Services;

public static class CodeGenTemplates
{
    public const string Entity = @"using JeeSiteNET.Core;
using {{ module_namespace }}.Domain.Entities;

namespace {{ module_namespace }}.Domain.Entities;

public class {{ class_name }} : {{ base_class }}
{
{{ for col in columns -}}
    public {{ col.net_type }}{{ if col.is_nullable == ""1"" && col.net_type != ""string"" && col.net_type != ""string?"" }}?{{ end }} {{ col.property_name }} { get; set; }{{ if col.net_type == ""string"" }} = string.Empty;{{ end }}
{{ end }}
}";

    public const string Configuration = @"using {{ module_namespace }}.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace {{ module_namespace }}.Infrastructure.EntityConfigurations;

public class {{ class_name }}Configuration : IEntityTypeConfiguration<{{ class_name }}>
{
    public void Configure(EntityTypeBuilder<{{ class_name }}> builder)
    {
        builder.ToTable(""{{ table_name }}"");
        builder.HasKey(e => e.{{ pk_name }});
{{ for col in columns -}}
{{ if col.max_length > 0 }}
        builder.Property(e => e.{{ col.property_name }}).HasMaxLength({{ col.max_length }});
{{ end -}}
{{ end }}
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
    }
}";

    public const string TreeEntity = @"using JeeSiteNET.Core;

namespace {{ module_namespace }}.Domain.Entities;

public class {{ class_name }} : TreeEntity
{
{{ for col in columns -}}
    public {{ col.net_type }}{{ if col.is_nullable == ""1"" && col.net_type != ""string"" && col.net_type != ""string?"" }}?{{ end }} {{ col.property_name }} { get; set; }{{ if col.net_type == ""string"" }} = string.Empty;{{ end }}
{{ end }}

    public override string GetName() => {{ tree_name_field }};
}";

    public const string TreeConfiguration = @"using {{ module_namespace }}.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace {{ module_namespace }}.Infrastructure.EntityConfigurations;

public class {{ class_name }}Configuration : IEntityTypeConfiguration<{{ class_name }}>
{
    public void Configure(EntityTypeBuilder<{{ class_name }}> builder)
    {
        builder.ToTable(""{{ table_name }}"");
        builder.HasKey(e => e.{{ pk_name }});
{{ for col in columns -}}
{{ if col.max_length > 0 }}
        builder.Property(e => e.{{ col.property_name }}).HasMaxLength({{ col.max_length }});
{{ end -}}
{{ end }}
        builder.Property(e => e.ParentCode).HasMaxLength(100);
        builder.Property(e => e.ParentCodes).HasMaxLength(2000);
        builder.Property(e => e.TreeSorts).HasMaxLength(2000);
        builder.Property(e => e.TreeNames).HasMaxLength(2000);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
    }
}";

    public const string RepositoryInterface = @"using JeeSiteNET.Core;
using {{ module_namespace }}.Domain.Entities;

namespace {{ module_namespace }}.Domain.Interfaces;

public interface I{{ class_name }}Repository : IRepository<{{ class_name }}>
{
}";

    public const string Repository = @"using JeeSiteNET.Core;
using {{ module_namespace }}.Domain.Entities;
using {{ module_namespace }}.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace {{ module_namespace }}.Infrastructure.Repositories;

public class {{ class_name }}Repository : I{{ class_name }}Repository
{
    private readonly JeeSiteDbContext _db;
    public {{ class_name }}Repository(JeeSiteDbContext db) => _db = db;
    public IQueryable<{{ class_name }}> Query() => _db.Set<{{ class_name }}>().AsNoTracking();
    public async Task<{{ class_name }}?> GetAsync(object id) => await _db.Set<{{ class_name }}>().FindAsync(id);
    public async Task<List<{{ class_name }}>> FindListAsync() => await _db.Set<{{ class_name }}>().AsNoTracking().ToListAsync();
    public async Task AddAsync({{ class_name }} entity) { _db.Set<{{ class_name }}>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync({{ class_name }} entity) { _db.Set<{{ class_name }}>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync({{ class_name }} entity) { _db.Set<{{ class_name }}>().Remove(entity); await _db.SaveChangesAsync(); }
}";

    public const string Service = @"using JeeSiteNET.Core;
using {{ module_namespace }}.Application.DTOs;
using {{ module_namespace }}.Domain.Entities;
using {{ module_namespace }}.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace {{ module_namespace }}.Application.Services;

public class {{ class_name }}Service
{
    private readonly I{{ class_name }}Repository _repository;
    public {{ class_name }}Service(I{{ class_name }}Repository repository) => _repository = repository;

    public async Task<{{ class_name }}Dto?> GetAsync({{ pk_net_type }} id)
    {
        var entity = await _repository.GetAsync(id);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<PageResult<{{ class_name }}Dto>> FindPageAsync(PageRequest<{{ class_name }}> request)
    {
        var query = _repository.Query().OrderBy(e => e.{{ pk_name }});
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<{{ class_name }}Dto> { List = list.Select(MapToDto).ToList(), Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }

    public async Task<ApiResult> SaveAsync({{ class_name }}SaveDto dto)
    {
        var now = DateTime.Now;
        {{ class_name }}? entity;
        if (!string.IsNullOrEmpty(dto.{{ pk_name }}?.ToString()))
        {
            entity = await _repository.GetAsync(dto.{{ pk_name }}!);
            if (entity == null) return ApiResult.NotFound(""{{ function_name }}不存在"");
{{ for col in columns -}}
{{ if col.is_pk == ""0"" && col.is_edit == ""1"" }}
            entity.{{ col.property_name }} = dto.{{ col.property_name }};
{{ end -}}
{{ end }}
            entity.UpdateDate = now;
            await _repository.UpdateAsync(entity);
        }
        else
        {
            entity = new {{ class_name }} { CreateDate = now, UpdateDate = now };
{{ for col in columns -}}
{{ if col.is_pk == ""0"" && col.is_insert == ""1"" }}
            entity.{{ col.property_name }} = dto.{{ col.property_name }};
{{ end -}}
{{ end }}
            await _repository.AddAsync(entity);
        }
        return ApiResult.Ok(MapToDto(entity));
    }

    public async Task<ApiResult> DeleteAsync({{ pk_net_type }} id)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null) return ApiResult.NotFound(""{{ function_name }}不存在"");
        await _repository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    private static {{ class_name }}Dto MapToDto({{ class_name }} e) => new()
    {
{{ for col in columns -}}
{{ if col.is_list == ""1"" }}
        {{ col.property_name }} = e.{{ col.property_name }},
{{ end -}}
{{ end }}
    };
}";

    public const string TreeService = @"using JeeSiteNET.Core;
using {{ module_namespace }}.Application.DTOs;
using {{ module_namespace }}.Domain.Entities;
using {{ module_namespace }}.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace {{ module_namespace }}.Application.Services;

public class {{ class_name }}Service
{
    private readonly I{{ class_name }}Repository _repository;
    public {{ class_name }}Service(I{{ class_name }}Repository repository) => _repository = repository;

    public async Task<List<{{ class_name }}Dto>> GetTreeAsync()
    {
        var list = await _repository.Query().OrderBy(e => e.TreeSort).ToListAsync();
        return list.Select(MapToDto).ToList();
    }

    public async Task<{{ class_name }}Dto?> GetAsync({{ pk_net_type }} id)
    {
        var entity = await _repository.GetAsync(id);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<ApiResult> SaveAsync({{ class_name }}SaveDto dto)
    {
        var now = DateTime.Now;
        {{ class_name }}? entity;
        if (!string.IsNullOrEmpty(dto.{{ pk_name }}?.ToString()))
        {
            entity = await _repository.GetAsync(dto.{{ pk_name }}!);
            if (entity == null) return ApiResult.NotFound(""{{ function_name }}不存在"");
{{ for col in columns -}}
{{ if col.is_pk == ""0"" && col.is_edit == ""1"" }}
            entity.{{ col.property_name }} = dto.{{ col.property_name }};
{{ end -}}
{{ end }}
            entity.UpdateDate = now;
            await _repository.UpdateAsync(entity);
        }
        else
        {
            entity = new {{ class_name }} { CreateDate = now, UpdateDate = now };
{{ for col in columns -}}
{{ if col.is_pk == ""0"" && col.is_insert == ""1"" }}
            entity.{{ col.property_name }} = dto.{{ col.property_name }};
{{ end -}}
{{ end }}
            await _repository.AddAsync(entity);
        }
        return ApiResult.Ok(MapToDto(entity));
    }

    public async Task<ApiResult> DeleteAsync({{ pk_net_type }} id)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null) return ApiResult.NotFound(""{{ function_name }}不存在"");
        await _repository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    private static {{ class_name }}Dto MapToDto({{ class_name }} e) => new()
    {
{{ for col in columns -}}
{{ if col.is_list == ""1"" }}
        {{ col.property_name }} = e.{{ col.property_name }},
{{ end -}}
{{ end }}
    };
}";

    public const string TreeDto = @"namespace {{ module_namespace }}.Application.DTOs;

public class {{ class_name }}Dto
{
{{ for col in columns -}}
{{ if col.is_list == ""1"" }}
    public {{ col.net_type }}{{ if col.is_nullable == ""1"" && col.net_type != ""string"" && col.net_type != ""string?"" }}?{{ end }} {{ col.property_name }} { get; set; }{{ if col.net_type == ""string"" }} = string.Empty;{{ end }}
{{ end -}}
{{ end }}
    public List<{{ class_name }}Dto> Children { get; set; } = [];
}

public class {{ class_name }}SaveDto
{
    public string? {{ pk_name }} { get; set; }
    public string? ParentCode { get; set; }
    public decimal? TreeSort { get; set; }
{{ for col in columns -}}
{{ if col.is_pk == ""0"" }}
    public {{ col.net_type }}{{ if col.is_nullable == ""1"" && col.net_type != ""string"" && col.net_type != ""string?"" }}?{{ end }} {{ col.property_name }} { get; set; }{{ if col.net_type == ""string"" }} = string.Empty;{{ end }}
{{ end -}}
{{ end }}
}";

    public const string Dto = @"namespace {{ module_namespace }}.Application.DTOs;

public class {{ class_name }}Dto
{
{{ for col in columns -}}
{{ if col.is_list == ""1"" }}
    public {{ col.net_type }}{{ if col.is_nullable == ""1"" && col.net_type != ""string"" && col.net_type != ""string?"" }}?{{ end }} {{ col.property_name }} { get; set; }{{ if col.net_type == ""string"" }} = string.Empty;{{ end }}
{{ end -}}
{{ end }}
}

public class {{ class_name }}SaveDto
{
{{ for col in columns -}}
{{ if col.is_pk == ""0"" }}
    public {{ col.net_type }}{{ if col.is_nullable == ""1"" && col.net_type != ""string"" && col.net_type != ""string?"" }}?{{ end }} {{ col.property_name }} { get; set; }{{ if col.net_type == ""string"" }} = string.Empty;{{ end }}
{{ end -}}
{{ end }}
}";

    public const string Controller = @"using JeeSiteNET.Core;
using {{ module_namespace }}.Application.DTOs;
using {{ module_namespace }}.Application.Services;
using {{ module_namespace }}.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace {{ module_namespace }}.Controllers;

[ApiController]
[Route(""api/v1/{{ module_lower }}/{{ business_name }}"")]
public class {{ class_name }}Controller : ControllerBase
{
    private readonly {{ class_name }}Service _service;
    public {{ class_name }}Controller({{ class_name }}Service service) => _service = service;

    [HttpPost(""list"")]
    [Permission(""{{ permission_prefix }}:list"")]
    public async Task<ApiResult<PageResult<{{ class_name }}Dto>>> List([FromBody] PageRequest<{{ class_name }}> request)
        => ApiResult<PageResult<{{ class_name }}Dto>>.Ok(await _service.FindPageAsync(request));

    [HttpGet(""get"")]
    [Permission(""{{ permission_prefix }}:view"")]
    public async Task<ApiResult<{{ class_name }}Dto?>> Get([FromQuery] {{ pk_net_type }} id)
    {
        var dto = await _service.GetAsync(id);
        return dto == null ? ApiResult<{{ class_name }}Dto?>.NotFound() : ApiResult<{{ class_name }}Dto?>.Ok(dto);
    }

    [HttpPost(""save"")]
    [Permission(""{{ permission_prefix }}:save"")]
    public async Task<ApiResult> Save([FromBody] {{ class_name }}SaveDto dto) => await _service.SaveAsync(dto);

    [HttpPost(""delete"")]
    [Permission(""{{ permission_prefix }}:delete"")]
    public async Task<ApiResult> Delete([FromBody] Delete{{ class_name }}Request request) => await _service.DeleteAsync(request.Id);
}

public class Delete{{ class_name }}Request { public {{ pk_net_type }} Id { get; set; } = default!; }";

    public const string TreeController = @"using JeeSiteNET.Core;
using {{ module_namespace }}.Application.DTOs;
using {{ module_namespace }}.Application.Services;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace {{ module_namespace }}.Controllers;

[ApiController]
[Route(""api/v1/{{ module_lower }}/{{ business_name }}"")]
public class {{ class_name }}Controller : ControllerBase
{
    private readonly {{ class_name }}Service _service;
    public {{ class_name }}Controller({{ class_name }}Service service) => _service = service;

    [HttpGet(""tree"")]
    [Permission(""{{ permission_prefix }}:list"")]
    public async Task<ApiResult<List<{{ class_name }}Dto>>> Tree()
        => ApiResult<List<{{ class_name }}Dto>>.Ok(await _service.GetTreeAsync());

    [HttpGet(""get"")]
    [Permission(""{{ permission_prefix }}:view"")]
    public async Task<ApiResult<{{ class_name }}Dto?>> Get([FromQuery] {{ pk_net_type }} id)
    {
        var dto = await _service.GetAsync(id);
        return dto == null ? ApiResult<{{ class_name }}Dto?>.NotFound() : ApiResult<{{ class_name }}Dto?>.Ok(dto);
    }

    [HttpPost(""save"")]
    [Permission(""{{ permission_prefix }}:save"")]
    public async Task<ApiResult> Save([FromBody] {{ class_name }}SaveDto dto) => await _service.SaveAsync(dto);

    [HttpPost(""delete"")]
    [Permission(""{{ permission_prefix }}:delete"")]
    public async Task<ApiResult> Delete([FromBody] Delete{{ class_name }}Request request) => await _service.DeleteAsync(request.Id);
}

public class Delete{{ class_name }}Request { public string Id { get; set; } = string.Empty; }";

    public const string QueryService = @"using JeeSiteNET.Core;
using {{ module_namespace }}.Application.DTOs;
using {{ module_namespace }}.Domain.Entities;
using {{ module_namespace }}.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace {{ module_namespace }}.Application.Services;

public class {{ class_name }}Service
{
    private readonly I{{ class_name }}Repository _repository;
    public {{ class_name }}Service(I{{ class_name }}Repository repository) => _repository = repository;

    public async Task<{{ class_name }}Dto?> GetAsync({{ pk_net_type }} id)
    {
        var entity = await _repository.GetAsync(id);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<PageResult<{{ class_name }}Dto>> FindPageAsync(PageRequest<{{ class_name }}> request)
    {
        var query = _repository.Query().OrderBy(e => e.{{ pk_name }});
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<{{ class_name }}Dto> { List = list.Select(MapToDto).ToList(), Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }

    private static {{ class_name }}Dto MapToDto({{ class_name }} e) => new()
    {
{{ for col in columns -}}
{{ if col.is_list == ""1"" }}
        {{ col.property_name }} = e.{{ col.property_name }},
{{ end -}}
{{ end }}
    };
}";

    public const string QueryController = @"using JeeSiteNET.Core;
using {{ module_namespace }}.Application.DTOs;
using JeeSiteNET.Modules.{{ module_code }}.Application.Services;
using JeeSiteNET.Modules.{{ module_code }}.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace {{ module_namespace }}.Controllers;

[ApiController]
[Route(""api/v1/{{ module_lower }}/{{ business_name }}"")]
public class {{ class_name }}Controller : ControllerBase
{
    private readonly {{ class_name }}Service _service;
    public {{ class_name }}Controller({{ class_name }}Service service) => _service = service;

    [HttpPost(""list"")]
    [Permission(""{{ permission_prefix }}:list"")]
    public async Task<ApiResult<PageResult<{{ class_name }}Dto>>> List([FromBody] PageRequest<{{ class_name }}> request)
        => ApiResult<PageResult<{{ class_name }}Dto>>.Ok(await _service.FindPageAsync(request));

    [HttpGet(""get"")]
    [Permission(""{{ permission_prefix }}:view"")]
    public async Task<ApiResult<{{ class_name }}Dto?>> Get([FromQuery] {{ pk_net_type }} id)
    {
        var dto = await _service.GetAsync(id);
        return dto == null ? ApiResult<{{ class_name }}Dto?>.NotFound() : ApiResult<{{ class_name }}Dto?>.Ok(dto);
    }
}";

    public const string ModuleInstaller = @"using {{ module_namespace }}.Application.Services;
using {{ module_namespace }}.Domain.Interfaces;
using {{ module_namespace }}.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace {{ module_namespace }};

[ModuleDescription(Code = ""{{ module_code }}"", Name = ""{{ function_name }}"", Version = ""1.0.0"")]
public class {{ class_name }}ModuleInstaller : IModuleInstaller
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<I{{ class_name }}Repository, {{ class_name }}Repository>();
        services.AddScoped<{{ class_name }}Service>();
    }
}";

    public const string VueList = @"<template>
  <a-card :bordered=""false"">
    <a-form layout=""inline"" :model=""queryParam"" @keyup.enter.native=""searchQuery"">
      <a-form-item label=""关键词"">
        <a-input v-model:value=""queryParam.keyword"" placeholder=""搜索"" allow-clear />
      </a-form-item>
      <a-form-item>
        <a-button type=""primary"" @click=""searchQuery"">查询</a-button>
        <a-button style=""margin-left: 8px"" @click=""resetQuery"">重置</a-button>
      </a-form-item>
    </a-form>
    <a-table
      :columns=""columns""
      :data-source=""dataList""
      :loading=""loading""
      :pagination=""pagination""
      row-key=""{{ vue_pk }}""
      @change=""handleTableChange""
    >
      <template #bodyCell=""{ text, record, index }, column}"">
        <template v-if=""column.key === 'action'"">
          <a @click=""handleEdit(record)"">编辑</a>
          <a-divider type=""vertical"" />
          <a-popconfirm title=""确定删除？"" @confirm=""handleDelete(record)"">
            <a>删除</a>
          </a-popconfirm>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open=""modalVisible"" title=""{{ function_name }}"" @ok=""handleOk"" :confirm-loading=""modalLoading"">
      <a-form :model=""formState"" :label-col=""{ span: 6 }"" :wrapper-col=""{ span: 16 }"">
{{ for col in vue_form_fields -}}
        <a-form-item label=""{{ col.comment }}"">
          <a-input v-model:value=""formState.{{ col.property_name }}"" />
        </a-form-item>
{{ end -}}
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang=""ts"">
import { ref, reactive, onMounted } from 'vue'
import { message } from 'ant-design-vue'
import axios from 'axios'

const dataList = ref<any[]>([])
const loading = ref(false)
const modalVisible = ref(false)
const modalLoading = ref(false)
const queryParam = reactive({ keyword: '' })
const pagination = reactive({ current: 1, pageSize: 20, total: 0 })
const formState = reactive<Record<string, any>>({})

const columns = [
{{ for col in vue_table_fields -}}
  { title: '{{ col.comment }}', dataIndex: '{{ col.property_name }}', key: '{{ col.property_name }}' },
{{ end -}}
  { title: '操作', key: 'action' }
]

const fetchData = async () => {
  loading.value = true
  try {
    const res = await axios.post('/api/v1/{{ module_lower }}/{{ business_name }}/list', {
      pageNo: pagination.current,
      pageSize: pagination.pageSize,
      entity: {}
    })
    if (res.data.code === 200) {
      dataList.value = res.data.data.list
      pagination.total = res.data.data.total
    }
  } finally {
    loading.value = false
  }
}

const searchQuery = () => { pagination.current = 1; fetchData() }
const resetQuery = () => { queryParam.keyword = ''; searchQuery() }
const handleTableChange = (pag: any) => { pagination.current = pag.current; pagination.pageSize = pag.pageSize; fetchData() }

const handleEdit = (record: any) => {
  Object.assign(formState, record)
  modalVisible.value = true
}

const handleDelete = async (record: any) => {
  await axios.post('/api/v1/{{ module_lower }}/{{ business_name }}/delete', { id: record.{{ vue_pk }} })
  message.success('删除成功')
  fetchData()
}

const handleOk = async () => {
  modalLoading.value = true
  try {
    await axios.post('/api/v1/{{ module_lower }}/{{ business_name }}/save', { ...formState })
    message.success('保存成功')
    modalVisible.value = false
    fetchData()
  } finally {
    modalLoading.value = false
  }
}

onMounted(fetchData)
</script>";

    public const string VueTree = @"<template>
  <a-card :bordered=""false"">
    <a-button type=""primary"" style=""margin-bottom:16px"" @click=""handleAdd()"">新增</a-button>
    <a-table :data-source=""formatTree(dataList)"" :columns=""columns"" :loading=""loading"" row-key=""{{ vue_pk }}""
      :pagination=""false"" :default-expand-all-rows=""true"">
      <template #bodyCell=""{ record, column }"">
        <template v-if=""column.key === 'action'"">
          <a @click=""handleEdit(record)"">编辑</a>
          <a-divider type=""vertical"" />
          <a-popconfirm title=""确定删除？"" @confirm=""handleDelete(record)"">
            <a>删除</a>
          </a-popconfirm>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open=""modalVisible"" title=""{{ function_name }}"" @ok=""handleOk"" :confirm-loading=""modalLoading"">
      <a-form :model=""formState"" :label-col=""{ span: 6 }"" :wrapper-col=""{ span: 16 }"">
        <a-form-item label=""上级"">
          <a-tree-select v-model:value=""formState.parentCode"" :tree-data=""formatTree(dataList)"" allow-clear
            tree-default-expand-all placeholder=""请选择上级"" />
        </a-form-item>
{{ for col in vue_form_fields -}}
        <a-form-item label=""{{ col.comment }}"">
          <a-input v-model:value=""formState.{{ col.property_name }}"" />
        </a-form-item>
{{ end -}}
      </a-form>
    </a-modal>
  </a-card>
</template>

<script setup lang=""ts"">
import { ref, onMounted } from 'vue'
import { message } from 'ant-design-vue'
import axios from 'axios'

const dataList = ref<any[]>([])
const loading = ref(false)
const modalVisible = ref(false)
const modalLoading = ref(false)
const formState = reactive<Record<string, any>>({})

const columns = [
{{ for col in vue_table_fields -}}
  { title: '{{ col.comment }}', dataIndex: '{{ col.property_name }}', key: '{{ col.property_name }}' },
{{ end -}}
  { title: '操作', key: 'action' }
]

function formatTree(list: any[], parentCode = '0'): any[] {
  return list.filter(i => i.parentCode === parentCode).map(i => ({
    ...i, key: i.{{ vue_pk }}, children: formatTree(list, i.{{ vue_pk }})
  }))
}

const fetchData = async () => {
  loading.value = true
  try {
    const res = await axios.get('/api/v1/{{ module_lower }}/{{ business_name }}/tree')
    if (res.data.code === 200) dataList.value = res.data.data
  } finally { loading.value = false }
}

const handleAdd = () => { Object.assign(formState, { parentCode: '0' }); modalVisible.value = true }
const handleEdit = (record: any) => { Object.assign(formState, record); modalVisible.value = true }

const handleDelete = async (record: any) => {
  await axios.post('/api/v1/{{ module_lower }}/{{ business_name }}/delete', { id: record.{{ vue_pk }} })
  message.success('删除成功'); fetchData()
}

const handleOk = async () => {
  modalLoading.value = true
  try {
    await axios.post('/api/v1/{{ module_lower }}/{{ business_name }}/save', { ...formState })
    message.success('保存成功'); modalVisible.value = false; fetchData()
  } finally { modalLoading.value = false }
}

onMounted(fetchData)
</script>";
}
