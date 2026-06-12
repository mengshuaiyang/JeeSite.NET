import type { Directive, DirectiveBinding } from 'vue'
import { usePermission } from '@/composables/usePermission'

function checkPermission(el: HTMLElement, binding: DirectiveBinding<string | string[]>) {
  const { has, hasAny } = usePermission()
  const value = binding.value

  if (value === undefined || value === null || value === '') return

  let passed = false
  if (Array.isArray(value)) {
    passed = hasAny(value)
  } else {
    passed = has(value)
  }

  if (!passed) {
    el.remove()
  }
}

const permissionDirective: Directive<HTMLElement, string | string[]> = {
  mounted(el, binding) {
    checkPermission(el, binding)
  },
  updated(el, binding) {
    if (binding.value !== binding.oldValue) {
      checkPermission(el, binding)
    }
  },
}

export default permissionDirective
