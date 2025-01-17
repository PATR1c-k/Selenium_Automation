import React, { useState } from "react";
import axios from "axios";
import {
  TextField,
  Button,
  Typography,
  Box,
  CircularProgress,
  Card,
  CardContent,
} from "@mui/material";

const Form = () => {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [target, setTarget] = useState("");
  const [responseMessage, setResponseMessage] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    // Create the form data object
    const formData = {
      name: name,
      description: description,
      target: target,
    };

    try {
      // Make a POST request to the backend
      const res = await axios.post("http://localhost:4501/api/forms", formData);

      // Handle success response
      setResponseMessage(`Form submitted successfully! ID: ${res.data.id}`);
    } catch (error) {
      // Handle error response
      setResponseMessage("Error submitting form.");
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        minHeight: "100vh",
        backgroundColor: "#f7f7f7", // Background color for the page
      }}
    >
      <Card sx={{ width: 400, padding: 3, boxShadow: 3, borderRadius: 3 }}>
        <CardContent>
          <Typography variant="h5" align="center" gutterBottom>
            Submit Scan Information
          </Typography>

          <form onSubmit={handleSubmit}>
            <TextField
              label="Name"
              variant="outlined"
              fullWidth
              margin="normal"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
              sx={{ marginBottom: 2 }}
            />

            <TextField
              label="Description"
              variant="outlined"
              fullWidth
              margin="normal"
              multiline
              rows={4}
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              required
              sx={{ marginBottom: 2 }}
            />

            <TextField
              label="Target"
              variant="outlined"
              fullWidth
              margin="normal"
              value={target}
              onChange={(e) => setTarget(e.target.value)}
              required
              sx={{ marginBottom: 2 }}
            />

            <Button
              variant="contained"
              color="primary"
              type="submit"
              fullWidth
              sx={{ marginTop: 2, padding: "10px 0" }}
              disabled={isLoading}
            >
              {isLoading ? (
                <CircularProgress size={24} color="inherit" />
              ) : (
                "Submit"
              )}
            </Button>
          </form>

          {responseMessage && (
            <Typography
              variant="body1"
              color={responseMessage.startsWith("Error") ? "error" : "success"}
              align="center"
              sx={{ marginTop: 2 }}
            >
              {responseMessage}
            </Typography>
          )}
        </CardContent>
      </Card>
    </Box>
  );
};

export default Form;
