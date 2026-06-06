<template>
  <a-card title="控制台">
    <a-result status="success" title="欢迎使用 JeeSite.NET 管理平台">
      <template #extra>
        <a-button type="primary" @click="$router.push('/sys/user')">进入用户管理</a-button>
      </template>
    </a-result>
  </a-card>
</template>
