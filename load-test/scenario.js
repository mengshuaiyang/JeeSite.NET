import http from 'k6/http';
import { check, sleep, group } from 'k6';
import { Rate, Trend } from 'k6/metrics';

const BASE_URL = __ENV.BASE_URL || 'http://localhost:5000/api/v1';
const TOKEN = __ENV.TOKEN || '';
const headers = TOKEN ? { Authorization: `Bearer ${TOKEN}` } : {};

const failRate = new Rate('failed_requests');
const loginTrend = new Trend('login_duration');
const listTrend = new Trend('list_duration');

export const options = {
  stages: [
    { target: 10, duration: '30s' },   // Ramp-up
    { target: 50, duration: '1m' },    // Steady
    { target: 0, duration: '30s' },    // Ramp-down
  ],
  thresholds: {
    failed_requests: ['rate<0.05'],     // < 5% failure
    http_req_duration: ['p(95)<2000'], // 95% under 2s
  },
};

export default function () {
  group('Auth', () => {
    const loginRes = http.post(`${BASE_URL}/sys/auth/login`, JSON.stringify({
      loginCode: 'admin', password: 'admin'
    }), { headers: { 'Content-Type': 'application/json' } });

    check(loginRes, { 'login success': (r) => r.status === 200 });
    loginTrend.add(loginRes.timings.duration);
    failRate.add(loginRes.status !== 200);
    sleep(1);
  });

  group('Dashboard', () => {
    const res = http.get(`${BASE_URL}/sys/dashboard/stats`, { headers });
    check(res, { 'dashboard ok': (r) => r.status === 200 });
    failRate.add(res.status !== 200);
  });

  group('User List', () => {
    const res = http.post(`${BASE_URL}/sys/user/list`, JSON.stringify({
      pageNo: 1, pageSize: 20
    }), { headers: { 'Content-Type': 'application/json', ...headers } });
    check(res, { 'user list ok': (r) => r.status === 200 });
    listTrend.add(res.timings.duration);
    failRate.add(res.status !== 200);
  });

  group('Health', () => {
    const res = http.get(`${BASE_URL}/health`);
    check(res, { 'health ok': (r) => r.status === 200 });
  });

  sleep(2);
}
