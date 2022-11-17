import html from 'template';

export default {
  template: html`<div class="common-layout">
    <el-container>
      <el-header>Header</el-header>
      <el-container>
        <el-aside width="200px">Aside</el-aside>
        <el-container>
          <el-main>Main</el-main>
          <el-footer>Footer</el-footer>
        </el-container>
      </el-container>
    </el-container>
  </div>`,
  styles: html`<style>
    .common-layout .el-header,
    .common-layout .el-footer {
      background-color: var(--el-color-primary-light-7);
      color: var(--el-text-color-primary);
      text-align: center;
    }
    .common-layout .el-aside {
      background-color: var(--el-color-primary-light-8);
      color: var(--el-text-color-primary);
      text-align: center;
    }
    .common-layout .el-main {
      background-color: var(--el-color-primary-light-9);
      color: var(--el-text-color-primary);
      text-align: center;
      height: 150px;
    }
  </style>`,
};