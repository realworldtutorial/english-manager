<script setup lang="ts">
import { useRoute } from 'vue-router'
import type { Post } from '~/types/post'

const route = useRoute()
const slug = route.params.slug as string

const { data: posts } = await useFetch<Post[]>('/api/posts')
const post = computed(() => posts.value?.find(p => p.slug === slug))

const backView = () => {
  const { $router } = useNuxtApp()
  $router.back()
}

definePageMeta({
  layout: 'blog'
})
</script>

<template>
  <div v-if="post">
    <h1>{{ post.title }}</h1>
    <p>{{ post.content }}</p>
    <a href="javascript:;" @click="backView()">Back</a>
  </div>
  <div v-else>Không tìm thấy bài viết</div>
</template>
