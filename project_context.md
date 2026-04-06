AI Mentor – Full Project Context (For Cursor IDE)
1️⃣ Project Vision

We are building a:

Production-grade RAG (Retrieval-Augmented Generation) AI system using .NET 8, OpenAI, PostgreSQL, and pgvector.

This is NOT a simple ChatGPT wrapper.

The goal is to learn:

LLM integration
Embeddings
Vector databases
Semantic search
Retrieval-Augmented Generation (RAG)
Streaming responses
Clean Architecture in .NET
Enterprise-grade AI system design

This project is meant to be:

Learning-focused
Architecturally correct
Production-style
Resume-level showcase
2️⃣ Technology Stack

Backend:

.NET 8
ASP.NET Core Web API
Clean Architecture
EF Core
PostgreSQL
pgvector
OpenAI SDK (v2.9.0)

Database:

PostgreSQL (Dockerized)
pgvector extension enabled

AI:

OpenAI Chat model
OpenAI Embedding model: text-embedding-3-small

Streaming:

Server-Sent Events (SSE)
IAsyncEnumerable
3️⃣ Architecture Pattern

Clean Architecture:

API → Infrastructure → Application → Domain
Layer Responsibilities
Domain

Contains:

Entities only
No external dependencies
Application

Contains:

Interfaces (ILLMService, IEmbeddingService, IVectorRepository, IRagService)
Business contracts
No EF Core
No OpenAI
Infrastructure

Contains:

OpenAI implementations
EF Core DbContext
VectorRepository
RagService implementation
Embedding service
Database logic
API

Contains:

Controllers
Endpoint definitions
DI registrations
4️⃣ What Has Been Implemented
✅ Phase 1 – LLM Integration
Implemented:
OpenAI Chat integration
Streaming responses using:
CompleteChatStreamingAsync
IAsyncEnumerable
SSE endpoint:
/api/chat/stream
What we learned:
Chat models
System prompts
Streaming token generation
Async streaming
Clean DI wiring
✅ Phase 2 – Embeddings + Vector Search
Implemented:
OpenAI embedding generation (v2.9.0 compatible)
PostgreSQL with pgvector
vector(1536) column
EF Core integration with pgvector
VectorRepository
Cosine similarity search
Store endpoint
Search endpoint
Database Schema

Documents:

Id (UUID)
Title
Content

DocumentChunks:

Id (UUID)
DocumentId (FK)
Content
Embedding (vector(1536))
What we learned:
Dense vector embeddings
Cosine similarity
Vector search
pgvector integration
EF Core custom column types
Foreign key integrity
✅ Phase 3 – RAG Pipeline
Implemented:

RagService:

Flow:

User Question
   ↓
Generate Embedding
   ↓
Search Top-K similar chunks
   ↓
Build context
   ↓
Inject context into system prompt
   ↓
Call LLM
   ↓
Stream response

Endpoint:

/api/chat/rag-stream
Prompt Design:

System prompt instructs:

Use ONLY retrieved context
If not found, respond with:
"I don't have enough information from the stored knowledge."
What we learned:
Retrieval-Augmented Generation
Context injection
Grounded responses
Hallucination mitigation
5️⃣ Current System Capabilities

The system can:

Stream AI responses
Generate embeddings
Store embeddings
Perform semantic similarity search
Inject retrieved context into LLM
Produce grounded AI responses

This is a working RAG engine.

6️⃣ Current Limitations

The system currently:

Stores each chunk as separate document (simplified)
Has no proper chunking pipeline
No ANN index (IVFFlat) yet
No hybrid search
No memory persistence
No evaluation metrics
No guardrails
No prompt injection protection
7️⃣ Next Engineering Steps (Roadmap)

We will now move toward enterprise-grade improvements.

🔥 Step 1 – Proper Document Ingestion Pipeline

Instead of:

Storing raw message as document

We will implement:

Full document ingestion
Chunking strategy (e.g., 500–800 tokens)
Overlapping chunks
Batch embedding
Proper document-to-chunks relationship

Learning goals:

Chunking strategies
Context window optimization
Token-aware segmentation
🔥 Step 2 – ANN Index (Performance Optimization)

Add:

CREATE INDEX ON "DocumentChunks"
USING ivfflat (Embedding vector_cosine_ops)
WITH (lists = 100);

Learning goals:

Approximate Nearest Neighbor
Index tuning
Performance trade-offs
🔥 Step 3 – Hybrid Search

Combine:

Vector similarity
Keyword search (tsvector)

Learning goals:

Hybrid retrieval
Re-ranking
Production search systems
🔥 Step 4 – Memory System

Add:

ChatHistory table
Short-term context window
Long-term vector memory
Stateful AI

Learning goals:

Conversational AI design
Context management
Token budgeting
🔥 Step 5 – Guardrails & Safety

Add:

Prompt injection detection
Context validation
Output moderation
Answer faithfulness checking

Learning goals:

AI security
Hallucination detection
Safe AI system design
8️⃣ Engineering Principles To Follow

When generating code:

Follow Clean Architecture strictly
Never introduce circular dependencies
Infrastructure implements Application interfaces
Application never references Infrastructure
Domain remains pure

Use:

Dependency Injection
Proper namespaces
Strong separation of concerns
9️⃣ Coding Standards
Use async/await properly
Use IAsyncEnumerable for streaming
Use constructor injection
Avoid static services
Keep DbContext in Infrastructure only
Use repository pattern for vector operations
🔟 Current Immediate Goal

We are now ready to implement:

Proper Document Ingestion + Chunking Pipeline

This is the next logical architectural upgrade.

1️⃣1️⃣ What We Are Learning Overall

This project teaches:

AI system architecture
Production RAG pipelines
Vector database integration
Clean Architecture with AI
Enterprise-ready backend design
Streaming LLM responses
Embedding pipelines
Retrieval optimization

This is far beyond beginner AI usage.

🎯 Final Objective

By the end of this project, we want:

A fully working RAG system
Proper ingestion pipeline
Optimized retrieval
Stateful memory
Guardrails
Production-ready architecture
Office-demo ready AI system
🚀 Current Stage Summary
Phase	Status
LLM Integration	✅
Streaming	✅
Embeddings	✅
Vector Search	✅
Basic RAG	✅
Ingestion Pipeline	🔜 Next
Memory System	🔜 Future
Guardrails	🔜 Future

You can paste this entire context into Cursor.

Now tell me:

When you open Cursor, do you want to:

1️⃣ Implement proper chunking pipeline first
2️⃣ Improve RAG prompt engineering
3️⃣ Add ANN indexing
4️⃣ Add memory system

We will proceed like senior AI engineers