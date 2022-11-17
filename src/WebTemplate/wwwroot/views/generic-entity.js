import html from 'template';

export default {
  template: html`<div class="width100 height100 flex-center">
    <el-card>
      <router-link to="/">{{$route.params.entity}}</router-link>
    </el-card>
  </div>`,
};
