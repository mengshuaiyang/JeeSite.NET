import { get } from './request'

export const oauth2Api = {
  getLoginUrl: (provider: string) =>
    `/api/v1/sys/auth/oauth2/${provider}?redirectUri=${encodeURIComponent(window.location.origin + '/oauth2/callback')}`,
}
