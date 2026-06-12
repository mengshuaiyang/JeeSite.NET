import type { HTMLAttributes } from 'vue'

declare module 'vue' {
  interface ComponentCustomProperties {}

  interface HTMLAttributes {
    vPermission?: string | string[]
  }
}

export {}
