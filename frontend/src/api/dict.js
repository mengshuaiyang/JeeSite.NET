import { post, get } from './request';
export const dictApi = {
    // DictType
    typeList: (data) => post('/sys/dict-type/list', data),
    typeGet: (dictTypeCode) => get('/sys/dict-type/get', { dictTypeCode }),
    typeSave: (data) => post('/sys/dict-type/save', data),
    typeDelete: (dictTypeCode) => post('/sys/dict-type/delete', { dictTypeCode }),
    // DictData
    dataList: (data) => post('/sys/dict-data/list', data),
    dataByType: (dictType) => get('/sys/dict-data/by-type', { dictType }),
    dataGet: (dictCode) => get('/sys/dict-data/get', { dictCode }),
    dataSave: (data) => post('/sys/dict-data/save', data),
    dataDelete: (dictCode) => post('/sys/dict-data/delete', { dictCode })
};
