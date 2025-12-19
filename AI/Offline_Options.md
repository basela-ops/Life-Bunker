# Research Summary: Offline / Local AI Options

**Developer:** Usman Chaudhry
**Date:** December 19, 2025

---

## Overview

This document summarizes three offline or local artificial intelligence technologies that can be integrated into the **Active** state of the AI State Machine. Each option enables AI processing without requiring an active internet connection once set up.

---

## 1. Ollama

### Description

Ollama is a local large language model server that allows AI models to run directly on a user’s machine. It is free, open-source, and designed for offline use after models are downloaded.

### Key Features

* Runs models such as **Llama 3**, **Mistral**, and **Phi-3**
* Simple JSON-based API
* Compatible with Windows, macOS, and Linux
* Fully offline after initial model installation

### Use Cases

* Natural language processing (NLP)
* Text generation and completion
* Question-answering systems

### Integration Method

* Send HTTP POST requests to `localhost:11434`
* Communicate using JSON request and response formats
* Easily callable from Unity using `UnityWebRequest`

---

## 2. Unity Sentis

### Description

Unity Sentis is Unity’s official neural network inference engine. It runs machine learning models directly inside the Unity engine without requiring an external server.

### Key Features

* Supports `.onnx` model files
* GPU-accelerated inference
* Cross-platform support (PC, mobile, and console)
* Low latency suitable for real-time applications

### Use Cases

* Image recognition and classification
* Object detection in games
* Real-time AI predictions during gameplay

### Integration Method

* Import `.onnx` models into the Unity project
* Use the Sentis API to execute inference
* Results are available within the same frame or the next frame

---

## 3. LM Studio

### Description

LM Studio is a desktop application that allows developers to run and test AI models locally using a graphical interface and an optional API server.

### Key Features

* User-friendly model browsing and downloading
* HTTP API compatible with OpenAI-style requests
* Built-in chat interface for testing
* Supports a wide range of open-source models

### Use Cases

* Rapid prototyping of AI features
* Testing and comparing different AI models
* Development and debugging

### Integration Method

* Start the LM Studio local server (default: `localhost:1234`)
* Send HTTP requests using an OpenAI-style API format
* Integrates easily with Unity networking tools

---

## Comparison

| Technology   | Best For              | Ease of Setup | Performance |
| ------------ | --------------------- | ------------- | ----------- |
| Ollama       | Production use        | Medium        | Fast        |
| Unity Sentis | Real-time Unity apps  | Easy          | Very Fast   |
| LM Studio    | Testing / Prototyping | Very Easy     | Fast        |

---

## Recommendation

For this AI State Machine project, **Ollama** is the recommended solution because it:

* Is free and well-documented
* Operates fully offline after setup
* Provides a simple and flexible API
* Supports multiple AI models without requiring code changes

---

## Conclusion

All three technologies offer effective solutions for running AI locally without internet dependency. The optimal choice depends on project requirements such as response time, platform compatibility, and ease of integration. For scalable and flexible local language processing, Ollama is the most suitable option for this project.
