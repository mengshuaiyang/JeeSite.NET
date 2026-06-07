import { post, get } from './request';
export const codegenApi = {
    list: (data) => post('/codegen/table/list', data),
    get: (tableName) => get('/codegen/table/get', { tableName }),
    save: (data) => post('/codegen/table/save', data),
    delete: (tableName) => post('/codegen/table/delete', { tableName }),
    dbTables: () => get('/codegen/table/db/tables'),
    dbColumns: (tableName) => get('/codegen/table/db/columns', { tableName }),
    importTables: (tableNames) => post('/codegen/table/import', { tableNames }),
    preview: (tableName) => get('/codegen/table/preview', { tableName })
};
