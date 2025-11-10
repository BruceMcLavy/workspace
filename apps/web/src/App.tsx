import { useEffect, useState } from "react";
import { Box } from "@mui/material"

type Note = { id: number; text: string };

export default function App() {
  const [health, setHealth] = useState("loading...");
  const [notes, setNotes] = useState<Note[]>([]);
  const [text, setText] = useState("");

  useEffect(() => {
    fetch("/api/health")
      .then(r => r.json()).then(d => setHealth(d.status))
      .catch(() => setHealth("error"));

    fetch("/api/notes")
      .then(r => r.json()).then(setNotes)
      .catch(() => setNotes([]));
  }, []);

  const addNote = async () => {
    if (!text.trim()) return;
    const res = await fetch("/api/notes", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ text })
    });
    const n: Note = await res.json();
    setNotes(prev => [...prev, n]);
    setText("");
  };

  return (
    <div style={{ padding: 20, fontFamily: "system-ui, sans-serif" }}>
      <h1>HQV – Web</h1>
      <p>API health status: <b>{health}</b></p>

      <h2>Notes</h2>
      <Box style={{ display: "flex", gap: 8, marginBottom: 12 }}>
        <input
          value={text}
          onChange={e => setText(e.target.value)}
          placeholder="New note..."
        />
        <button onClick={addNote}>Add Note</button>
      </Box>

      <ul>
        {notes.map(n => <li key={n.id}>{n.text}</li>)}
      </ul>
    </div>
  );
}
