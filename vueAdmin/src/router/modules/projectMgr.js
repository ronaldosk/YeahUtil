/** When your routing table is too long, you can split it into small modules**/

import Layout from '@/views/layout/Layout'

const projectMgrRouter = {
  path: '/table',
  component: Layout,
  redirect: '/table/complex-table',
  name: 'ProjectMgr',
  meta: {
    title: '项目管理',
    icon: 'table'
  },
  children: [
    {
      path: 'create',
      component: () => import('@/views/ProjectMgr/create'),
      name: 'CreateArticle',
      meta: { title: '新建项目', icon: 'edit' }
    },
    {
      path: 'list',
      component: () => import('@/views/ProjectMgr/complexTable'),
      name: 'ComplexTable',
      meta: { title: '项目列表', icon: 'list' }
    }
  ]
}
export default projectMgrRouter
