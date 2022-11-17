import html from 'template';

import Preview from '../components/preview.js';

const template = html`
  <el-tabs>
    <el-tab-pane label="Basic 基础组件">
      <el-tabs tab-position="left">
        <el-tab-pane label="Button 按钮">
          <el-row class="mb-4">
            <el-button>Default</el-button>
            <el-button type="primary">Primary</el-button>
            <el-button type="success">Success</el-button>
            <el-button type="info">Info</el-button>
            <el-button type="warning">Warning</el-button>
            <el-button type="danger">Danger</el-button>
            <el-button>中文</el-button>
          </el-row>
        </el-tab-pane>
        <el-tab-pane label="Border 边框">
          <el-space>
            <div style="width:10rem;height:5rem;border:1px solid var(--el-border-color);"></div>
            <div style="width:10rem;height:5rem;border:1px solid var(--el-border-color);border-radius: var(--el-border-radius-small)"></div>
            <div style="width:10rem;height:5rem;border:1px solid var(--el-border-color);border-radius: var(--el-border-radius-large )"></div>
            <div style="width:10rem;height:5rem;border:1px solid var(--el-border-color);border-radius: var(--el-border-radius-round )"></div>
          </el-space>
        </el-tab-pane>
        <el-tab-pane label="Color 色彩">
          <preview name="demo/color" />
        </el-tab-pane>
        <el-tab-pane label="Container 布局容器">
          <preview name="demo/container" />
        </el-tab-pane>
        <el-tab-pane label="Icon 图标">
          <el-link src="https://element-plus.gitee.io/zh-CN/component/icon.html">文档</el-link>
        </el-tab-pane>
        <el-tab-pane label="Layout 布局">
          <preview name="demo/layout" />
        </el-tab-pane>
        <el-tab-pane label="Link 链接">
          <el-link src="https://element-plus.gitee.io/zh-CN/component/link.html">文档</el-link>
        </el-tab-pane>
        <el-tab-pane label="Scrollbar 滚动条">
          <el-scrollbar height="400px">
            <p
              v-for="item in 20"
              :key="item"
              class="scrollbar-demo-item"
              style="display: flex;
                    align-items: center;
                    justify-content: center;
                    height: 50px;
                    margin: 10px;
                    text-align: center;
                    border-radius: 4px;
                    background: var(--el-color-primary-light);
                    color: var(--el-color-primary);"
            >
              {{ item }}
            </p>
          </el-scrollbar>
        </el-tab-pane>
        <el-tab-pane label="Space 间距">
          <el-space wrap>
            <el-card v-for="i in 3" :key="i" class="box-card" style="width: 250px">
              <template #header>
                <div class="card-header">
                  <span>Card name</span>
                  <el-button class="button" type="text">Operation button</el-button>
                </div>
              </template>
              <div v-for="o in 4" :key="o" class="text item"> {{ 'List item ' + o }} </div>
            </el-card>
          </el-space>
        </el-tab-pane>
        <el-tab-pane label="Typography 排版">
          <el-link src="https://element-plus.gitee.io/zh-CN/component/typography.html">文档</el-link>
        </el-tab-pane>
      </el-tabs>
    </el-tab-pane>
    <el-tab-pane label="配置组件">
      <el-tabs tab-position="left">
        <el-tab-pane label="Config Provider 全局配置">
          <el-link src="https://element-plus.gitee.io/zh-CN/component/config-provider.html">Config Provider 全局配置</el-link>
        </el-tab-pane>
      </el-tabs>
    </el-tab-pane>
    <el-tab-pane label="Form 表单组件">
      <el-tabs tab-position="left">
        <el-tab-pane label="Cascader 级联选择器">
          <preview name="demo/cascader" />
        </el-tab-pane>
        <el-tab-pane label="Checkbox 多选框">
          <el-checkbox label="Option 1" size="large" />
        </el-tab-pane>
        <el-tab-pane label="ColorPicker 颜色选择器">
          <el-color-picker />
        </el-tab-pane>
        <el-tab-pane label="DatePicker 日期选择器">
          <el-date-picker type="date" placeholder="Pick a day" />
        </el-tab-pane>
        <el-tab-pane label="DateTimePicker 日期时间选择器">
          <el-date-picker type="datetime" placeholder="Select date and time" />
        </el-tab-pane>
        <el-tab-pane label="Form 表单">
          <preview name="demo/form" />
        </el-tab-pane>
        <el-tab-pane label="Input 输入框">
          <el-input placeholder="Please input" />
        </el-tab-pane>
        <el-tab-pane label="Input Number 数字输入框">
          <el-input-number :min="1" :max="10" />
        </el-tab-pane>
        <el-tab-pane label="Radio 单选框">
          <el-radio label="1" size="large">Option 1</el-radio>
        </el-tab-pane>
        <el-tab-pane label="Rate 评分">
          <el-rate />
        </el-tab-pane>
        <el-tab-pane label="Select 选择器">
          <el-select placeholder="Select" size="large">
            <el-option key="value" label="label" value="value" />
          </el-select>
        </el-tab-pane>
        <el-tab-pane label="Select V2 虚拟列表选择器">
          <el-select-v2 placeholder="Select" size="large" :options="[{value:'value',label:'label'}]" />
        </el-tab-pane>
        <el-tab-pane label="Slider 滑块[dark mode error]">
          <div style="padding:2rem;">
            <el-slider />
          </div>
        </el-tab-pane>
        <el-tab-pane label="Switch 开关">
          <el-switch />
        </el-tab-pane>
        <el-tab-pane label="TimePicker 时间选择器">
          <el-time-picker placeholder="Arbitrary time" />
        </el-tab-pane>
        <el-tab-pane label="TimeSelect 时间选择">
          <el-time-select start="08:30" step="00:15" end="18:30" placeholder="Select time" />
        </el-tab-pane>
        <el-tab-pane label="Transfer 穿梭框[error]">
          <preview name="demo/transfer" />
        </el-tab-pane>
        <el-tab-pane label="Transfer 穿梭框[error]">
          <preview name="demo/transfer" />
        </el-tab-pane>
        <el-tab-pane label="Upload 上传">
          <el-upload><el-button type="primary">Click to upload</el-button></el-upload>
        </el-tab-pane>
      </el-tabs>
    </el-tab-pane>
    <el-tab-pane label="Data 数据展示">
      <el-tabs tab-position="left">
        <el-tab-pane label="Calendar 日历">
          <el-calendar />
        </el-tab-pane>
        <el-tab-pane label="Carousel 走马灯">
          <el-carousel height="150px">
            <el-carousel-item v-for="item in 4" :key="item">
              <h3 class="small">{{ item }}</h3>
            </el-carousel-item>
          </el-carousel>
        </el-tab-pane>
        <el-tab-pane label="Collapse 折叠面板">
          <el-collapse>
            <el-collapse-item title="Consistency" name="1">
              <div>
                Consistent with real life: in line with the process and logic of real life, and comply with languages and habits that the users are used to;
              </div>
              <div> Consistent within interface: all elements should be consistent, such as: design style, icons and texts, position of elements, etc. </div>
            </el-collapse-item>
            <el-collapse-item title="Feedback" name="2">
              <div> Operation feedback: enable the users to clearly perceive their operations by style updates and interactive effects; </div>
              <div> Visual feedback: reflect current state by updating or rearranging elements of the page. </div>
            </el-collapse-item>
            <el-collapse-item title="Efficiency" name="3">
              <div> Simplify the process: keep operating process simple and intuitive; </div>
              <div> Definite and clear: enunciate your intentions clearly so that the users can quickly understand and make decisions; </div>
              <div>
                Easy to identify: the interface should be straightforward, which helps the users to identify and frees them from memorizing and recalling.
              </div>
            </el-collapse-item>
            <el-collapse-item title="Controllability" name="4">
              <div> Decision making: giving advices about operations is acceptable, but do not make decisions for the users; </div>
              <div>
                Controlled consequences: users should be granted the freedom to operate, including canceling, aborting or terminating current operation.
              </div>
            </el-collapse-item>
          </el-collapse>
        </el-tab-pane>
        <el-tab-pane label="Descriptions 描述列表">
          <el-descriptions title="User Info">
            <el-descriptions-item label="Username">kooriookami</el-descriptions-item>
            <el-descriptions-item label="Telephone">18100000000</el-descriptions-item>
            <el-descriptions-item label="Place">Suzhou</el-descriptions-item>
            <el-descriptions-item label="Remarks">
              <el-tag size="small">School</el-tag>
            </el-descriptions-item>
            <el-descriptions-item label="Address">No.1188, Wuzhong Avenue, Wuzhong District, Suzhou, Jiangsu Province</el-descriptions-item>
          </el-descriptions>
        </el-tab-pane>
        <el-tab-pane label="Empty 空状态">
          <el-empty description="description" />
        </el-tab-pane>
        <el-tab-pane label="Image 图片">
          <el-image style="width: 100px; height: 100px" />
        </el-tab-pane>
        <el-tab-pane label="Infinite Scroll 无限滚动[error]">
          <preview name="demo/infinite" />
        </el-tab-pane>
        <el-tab-pane label="Pagination 分页">
          <el-pagination background layout="total, sizes, prev, pager, next, jumper" :total="1000" />
        </el-tab-pane>
        <el-tab-pane label="Progress 进度条">
          <div class="demo-progress">
            <el-progress :percentage="50" />
            <el-progress :percentage="100" format="50%" />
            <el-progress :percentage="100" status="success" />
            <el-progress :percentage="100" status="warning" />
            <el-progress :percentage="50" status="exception" />
          </div>
        </el-tab-pane>
        <el-tab-pane label="Result 结果">
          <el-row>
            <el-col :sm="12" :lg="6">
              <el-result icon="success" title="Success Tip" sub-title="Please follow the instructions">
                <template #extra>
                  <el-button type="primary">Back</el-button>
                </template>
              </el-result>
            </el-col>
            <el-col :sm="12" :lg="6">
              <el-result icon="warning" title="Warning Tip" sub-title="Please follow the instructions">
                <template #extra>
                  <el-button type="primary">Back</el-button>
                </template>
              </el-result>
            </el-col>
            <el-col :sm="12" :lg="6">
              <el-result icon="error" title="Error Tip" sub-title="Please follow the instructions">
                <template #extra>
                  <el-button type="primary">Back</el-button>
                </template>
              </el-result>
            </el-col>
            <el-col :sm="12" :lg="6">
              <el-result icon="info" title="Info Tip">
                <template #sub-title>
                  <p>Using slot as subtitle</p>
                </template>
                <template #extra>
                  <el-button type="primary">Back</el-button>
                </template>
              </el-result>
            </el-col>
          </el-row>
        </el-tab-pane>
        <el-tab-pane label="Skeleton 骨架屏[dark model error]">
          <el-skeleton />
          <br />
          <el-skeleton style="--el-skeleton-circle-size: 100px">
            <template #template>
              <el-skeleton-item variant="circle" />
            </template>
          </el-skeleton>
        </el-tab-pane>
        <el-tab-pane label="Table 表格">
          <preview name="demo/table" />
        </el-tab-pane>
        <el-tab-pane label="Tag 标签[dark mode error]">
          <el-tag>Tag 1</el-tag>
          <el-tag class="ml-2" type="success">Tag 2</el-tag>
          <el-tag class="ml-2" type="info">Tag 3</el-tag>
          <el-tag class="ml-2" type="warning">Tag 4</el-tag>
          <el-tag class="ml-2" type="danger">Tag 5</el-tag>
        </el-tab-pane>
        <el-tab-pane label="Timeline 时间线">
          <el-timeline>
            <el-timeline-item key="0" timestamp="2018-04-15">Event start </el-timeline-item>
            <el-timeline-item key="1" timestamp="2018-04-13">Approved </el-timeline-item>
            <el-timeline-item key="2" timestamp="2018-04-11">Success </el-timeline-item>
          </el-timeline>
        </el-tab-pane>
        <el-tab-pane label="Tree 树形控件">
          <preview name="demo/tree" />
        </el-tab-pane>
        <el-tab-pane label="TreeSelect 树形选择[error]">
          <preview name="demo/tree-select" />
        </el-tab-pane>
        <el-tab-pane label="Tree V2 虚拟化树形控件[error]">
          <preview name="demo/tree-v2" />
        </el-tab-pane>
      </el-tabs>
    </el-tab-pane>
    <el-tab-pane label="Navigation 导航">
      <el-tabs tab-position="left">
        <el-tab-pane label="Affix 固钉">
          <el-affix :offset="150">
            <el-button type="primary">Offset top 150px</el-button>
          </el-affix>
        </el-tab-pane>
        <el-tab-pane label="Backtop 回到顶部">
          <div class="backtop" style="height:400px;overflow:auto;">
            <p style="height:800px;">Scroll down to see the bottom-right button.</p>
            <el-backtop :right="100" :bottom="100" target=".backtop" />
          </div>
        </el-tab-pane>
        <el-tab-pane label="Breadcrumb 面包屑">
          <el-breadcrumb separator="/">
            <el-breadcrumb-item :to="{ path: '/' }">homepage</el-breadcrumb-item>
            <el-breadcrumb-item><a href="/">promotion management</a></el-breadcrumb-item>
            <el-breadcrumb-item>promotion list</el-breadcrumb-item>
            <el-breadcrumb-item>promotion detail</el-breadcrumb-item>
          </el-breadcrumb>
        </el-tab-pane>
        <el-tab-pane label="Dropdown 下拉菜单">
          <el-dropdown>
            <span class="el-dropdown-link"> Dropdown List </span>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item>Action 1</el-dropdown-item>
                <el-dropdown-item>Action 2</el-dropdown-item>
                <el-dropdown-item>Action 3</el-dropdown-item>
                <el-dropdown-item disabled>Action 4</el-dropdown-item>
                <el-dropdown-item divided>Action 5</el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
        </el-tab-pane>
        <el-tab-pane label="Menu 菜单">
          <el-menu default-active="1" mode="horizontal" :ellipsis="false">
            <el-menu-item index="1">Processing Center</el-menu-item>
            <el-sub-menu index="2">
              <template #title>Workspace</template>
              <el-menu-item index="2-1">item one</el-menu-item>
              <el-menu-item index="2-2">item two</el-menu-item>
              <el-menu-item index="2-3">item three</el-menu-item>
              <el-sub-menu index="2-4">
                <template #title>item four</template>
                <el-menu-item index="2-4-1">item one</el-menu-item>
                <el-menu-item index="2-4-2">item two</el-menu-item>
                <el-menu-item index="2-4-3">item three</el-menu-item>
              </el-sub-menu>
            </el-sub-menu>
            <el-menu-item index="3" disabled>Info</el-menu-item>
            <el-menu-item index="4">Orders</el-menu-item>
          </el-menu>
        </el-tab-pane>
        <el-tab-pane label="Page Header 页头">
          <el-page-header content="detail" />
        </el-tab-pane>
        <el-tab-pane label="Steps 步骤条">
          <el-steps :active="0" finish-status="success">
            <el-step title="Step 1" />
            <el-step title="Step 2" />
            <el-step title="Step 3" />
          </el-steps>
        </el-tab-pane>
        <el-tab-pane label="Tabs 标签页">
          <el-tabs type="card">
            <el-tab-pane label="User" name="first">User</el-tab-pane>
            <el-tab-pane label="Config" name="second">Config</el-tab-pane>
            <el-tab-pane label="Role" name="third">Role</el-tab-pane>
            <el-tab-pane label="Task" name="fourth">Task</el-tab-pane>
          </el-tabs>
        </el-tab-pane>
      </el-tabs>
    </el-tab-pane>
    <el-tab-pane label="Feedback 反馈组件">
      <el-tabs tab-position="left">
        <el-tab-pane label="Alert 提示[dark mode error]">
          <el-alert title="success alert" type="success" />
          <el-alert title="info alert" type="info" />
          <el-alert title="warning alert" type="warning" />
          <el-alert title="error alert" type="error" />
        </el-tab-pane>
        <el-tab-pane label="Dialog 对话框">
          <preview name="demo/dialog" />
        </el-tab-pane>
        <el-tab-pane label="Drawer 抽屉">
          <preview name="demo/drawer" />
        </el-tab-pane>
        <el-tab-pane label="Loading 加载">
          <el-table v-loading="true" style="width: 100%"> </el-table>
        </el-tab-pane>
        <el-tab-pane label="Message 消息提示[dark mode error]">
          <preview name="demo/message" />
        </el-tab-pane>
        <el-tab-pane label="Notification 通知">
          <preview name="demo/notification" />
        </el-tab-pane>
        <el-tab-pane label="Popconfirm 气泡确认框">
          <el-popconfirm title="Are you sure to delete this?">
            <template #reference>
              <el-button>Delete</el-button>
            </template>
          </el-popconfirm>
        </el-tab-pane>
        <el-tab-pane label="Popover 气泡卡片">
          <el-popover placement="top-start" title="Title" :width="200" trigger="hover" content="this is content, this is content, this is content">
            <template #reference>
              <el-button>Hover to activate</el-button>
            </template>
          </el-popover>
        </el-tab-pane>
        <el-tab-pane label="Tooltip 文字提示">
          <el-tooltip class="box-item" effect="dark" content="Top Left prompts info" placement="top-start">
            <el-button>top-start</el-button>
          </el-tooltip>
        </el-tab-pane>
      </el-tabs>
    </el-tab-pane>
    <el-tab-pane label="Others 其他">
      <el-tabs tab-position="left">
        <el-tab-pane label="Divider 分割线">
          <div>
            <span>I sit at my window this morning where the world like a passer-by stops for a moment, nods to me and goes.</span>
            <el-divider content-position="left">Rabindranath Tagore</el-divider>
            <span>There little thoughts are the rustle of leaves; they have their whisper of joy in my mind.</span>
          </div>
        </el-tab-pane>
      </el-tabs>
    </el-tab-pane>
  </el-tabs>
`;
export default {
  template,
  components: { Preview },
};
