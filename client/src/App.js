import { useMemo, useState } from "react";
import "./App.css";
import { MOCK_EMAILS } from "./data/mockEmails";

function getRowClass(score) {
  if (score > 70) return "row high";
  if (score < 30) return "row low";
  return "row";
}

function formatTime(dt) {
  try {
    return new Date(dt).toLocaleString();
  } catch {
    return "";
  }
}

export default function App() {
  const API_URL = useMemo(() => "https://localhost:5001/api/inbox/sort", []);

  const [rawEmails, setRawEmails] = useState([]);
  const [sortedEmails, setSortedEmails] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [lastSortedAt, setLastSortedAt] = useState("");

  const loadMock = () => {
    setError("");
    setSortedEmails([]);
    setLastSortedAt("");
    setRawEmails(MOCK_EMAILS);
  };

  const scoreAndSort = async () => {
    setError("");
    setLoading(true);
    setSortedEmails([]);
    setLastSortedAt("");

    try {
      const res = await fetch(API_URL, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(rawEmails)
      });

      if (!res.ok) {
        const text = await res.text();
        throw new Error(text || `HTTP ${res.status}`);
      }

      const data = await res.json();
      setSortedEmails(data);
      setLastSortedAt(new Date().toISOString());
    } catch (e) {
      setError(
        "Couldn't reach the backend.\n\n" +
          "Quick checks:\n" +
          "* Is the server running?\n" +
          "* Is the API URL/port correct?\n" +
          "* HTTP vs HTTPS match?\n" +
          "* CORS enabled?\n\n" +
          String(e.message || e)
      );
    } finally {
      setLoading(false);
    }
  };

  const rawCount = rawEmails.length;
  const sortedCount = sortedEmails.length;

  return (
    <div className="container">
      <h1>Smart Inbox</h1>
      <p className="subtitle">
        Load a sample inbox, send it to the backend, and get a priority-sorted
        list back.
      </p>

      <div className="actions">
        <button onClick={loadMock}>Load sample inbox</button>
        <button onClick={scoreAndSort} disabled={rawCount === 0 || loading}>
          {loading ? "Sorting..." : "Prioritize emails"}
        </button>
      </div>

      {(rawCount > 0 || sortedCount > 0) && (
        <p className="subtitle" style={{ marginTop: 0 }}>
          {rawCount > 0 && <span>{rawCount} email(s) loaded.</span>}
          {sortedCount > 0 && (
            <span>
              {" "}
              Sorted {sortedCount} email(s)
              {lastSortedAt ? ` * Updated: ${formatTime(lastSortedAt)}` : ""}.
            </span>
          )}
        </p>
      )}

      {error && <pre className="error">{error}</pre>}

      <div className="panel">
        <h2>Raw Emails</h2>
        {rawCount === 0 ? (
          <div className="empty">
            Tip: click <b>"Load sample inbox"</b> to start.
          </div>
        ) : (
          <pre className="json">{JSON.stringify(rawEmails, null, 2)}</pre>
        )}
      </div>

      <div className="panel">
        <h2>Sorted Results</h2>
        {sortedCount === 0 ? (
          <div className="empty">
            When you're ready, click <b>"Prioritize emails"</b>.
          </div>
        ) : (
          <div className="list">
            {sortedEmails.map((e, idx) => (
              <div key={idx} className={getRowClass(e.priorityScore)}>
                <div className="left">
                  <div className="subject">{e.subject}</div>
                  <div className="meta">
                    <span>
                      <b>From:</b> {e.sender}
                    </span>
                    <span style={{ marginLeft: 10 }}>
                      <b>Score:</b> {e.priorityScore}
                    </span>
                  </div>
                </div>
                <div className="score">{e.priorityScore}</div>
              </div>
            ))}
          </div>
        )}

        <div className="legend">
          <span className="pill high">High &gt; 70</span>
          <span className="pill low">Low &lt; 30</span>
        </div>
      </div>
    </div>
  );
}
