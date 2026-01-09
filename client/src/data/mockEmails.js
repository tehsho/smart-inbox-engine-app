
const hoursAgo = (hours) =>
  new Date(Date.now() - hours * 60 * 60 * 1000).toISOString();

export const MOCK_EMAILS = [
  {
    sender: "marketing@store.com",
    subject: "Weekly Newsletter",
    body: "Click here to unsubscribe.",
    receivedAt: hoursAgo(6), // 6 hours ago
    isVIP: false
  },
  {
    sender: "boss@company.com",
    subject: "URGENT: Production DB Down",
    body: "Please fix this immediately.",
    receivedAt: hoursAgo(1), // 1 hour ago
    isVIP: true
  },
  {
    sender: "client@example.com",
    subject: "Question about invoice",
    body: "Can we meet later?",
    receivedAt: hoursAgo(24), // 1 day ago
    isVIP: false
  }
];
