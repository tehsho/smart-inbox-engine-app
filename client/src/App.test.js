import { render, screen, fireEvent } from "@testing-library/react";
import App from "./App";

describe("Smart Inbox App", () => {
  test("renders Smart Inbox title", () => {
    render(<App />);
    expect(
      screen.getByRole("heading", { name: /smart inbox/i })
    ).toBeInTheDocument();
  });

  test("loads mock emails when button is clicked", () => {
    render(<App />);

    const loadButton = screen.getByRole("button", {
      name: /load sample inbox/i
    });
    fireEvent.click(loadButton);

    expect(
      screen.getByText(/URGENT: Production DB Down/i)
    ).toBeInTheDocument();

    expect(screen.getByText(/Weekly Newsletter/i)).toBeInTheDocument();

    expect(
      screen.getByText(/Question about invoice/i)
    ).toBeInTheDocument();

    expect(screen.getByText(/email\(s\) loaded/i)).toBeInTheDocument();
  });

  test("Prioritize emails button is disabled until mock data is loaded", () => {
    render(<App />);

    const prioritizeButton = screen.getByRole("button", {
      name: /prioritize emails/i
    });
    expect(prioritizeButton).toBeDisabled();

    const loadButton = screen.getByRole("button", {
      name: /load sample inbox/i
    });
    fireEvent.click(loadButton);

    expect(prioritizeButton).not.toBeDisabled();
  });
});
