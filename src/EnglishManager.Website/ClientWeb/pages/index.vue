<script setup lang="ts">
import type { Post } from '~/types/post'

const { data: posts, pending } = await useAsyncData<Post[]>('posts', () => $fetch('/api/posts'))
</script>

<template>
  <div>
    <h1>📝 Bài viết mới nhất</h1>
    <div v-if="pending">Đang tải...</div>
    <ul v-else>
      <li v-for="post in posts" :key="post.id">
        <NuxtLink :to="`/posts/${post.slug}`">{{ post.title }}</NuxtLink>
      </li>
    </ul>
  </div>
</template>
