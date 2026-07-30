const test = require('node:test')
const assert = require('node:assert/strict')
const { classifyIssue, chooseRoute, rankIssue } = require('./issue-automation')
const { desiredStatus } = require('./project-sync')

test('classifies cross-stack database requirements as high complexity', () => {
  const result = classifyIssue(
    'Add a guest booking amendment screen',
    `Update the React page and the .NET API. Persist changes to the database.

- [ ] Guest can change dates
- [ ] API rejects conflicts
- [ ] Database is updated
- [ ] UI displays validation`
  )

  assert.equal(result.area, 'area:full-stack')
  assert.equal(result.complexity, 'complexity:high')
})

test('routes a low complexity frontend feature to Copilot on GitHub', () => {
  const route = chooseRoute([
    'area:frontend',
    'kind:feature',
    'complexity:low',
    'priority:p3'
  ])

  assert.equal(route.label, 'agent:copilot-app')
})

test('routes scripts to Copilot CLI', () => {
  const route = chooseRoute([
    'area:automation',
    'kind:script',
    'complexity:medium'
  ])

  assert.equal(route.label, 'agent:copilot-cli')
})

test('ranks urgent issues above normal issues', () => {
  const now = new Date().toISOString()
  const p1 = { labels: [{ name: 'priority:p1' }, { name: 'complexity:low' }], created_at: now }
  const p3 = { labels: [{ name: 'priority:p3' }, { name: 'complexity:high' }], created_at: now }

  assert.ok(rankIssue(p1) > rankIssue(p3))
})

test('keeps release-approved work in QA until release succeeds', () => {
  assert.equal(
    desiredStatus(['ready-for-qa', 'release-to-production']),
    'QA'
  )
  assert.equal(
    desiredStatus(['ready-for-qa', 'release-to-production', 'released']),
    'Done'
  )
})
