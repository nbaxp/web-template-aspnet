import html from 'template';
import { ref } from 'vue';

const template = html`<el-table ref="tableRef" highlight-current-row style="width: 100%">
  <el-table-column fixed type="selection"></el-table-column>
  <template v-for="(value,key) in schema.properties">
    <el-table-column :column-key="key" :prop="key" :label="value.title"></el-table-column>
  </template>
  <el-table-column fixed="right" label="操作" width="150">
    <template #default="scope">
      <el-button type="text" size="small" @click.prevent="deleteRow(scope.$index)"> 详情 </el-button>
      <el-button type="text" size="small" @click.prevent="deleteRow(scope.$index)"> 删除 </el-button>
    </template>
  </el-table-column>
</el-table>`;
export default {
  template,
  props: {
    schema: {
      type: Object,
      default: {},
    },
  },
  setup() {
    const tableRef = ref(null);
    return {
      tableRef,
    };
  },
};