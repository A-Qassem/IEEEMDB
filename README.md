# IEEEMDB

## Functional Requirements

**Admin:**
1. Admin can add new movies.
2. Admin can edit or delete existing movies.

**User:**
1. User can register on the platform.
2. User can review and rate movies.
3. User has a profile that contains their reviews, ratings, and watch history.
4. User can create movie lists and edit or delete from this list or share it with other users.
5. User receives notifications (new movies).

**Out of Scope:**
1. User receives notifications for social interactions.
2. User can make reports on movies.
3. Admin can view reports of users.

## Non-Functional Requirements

1. The system must be available 24/7.
2. The ability to use the system by millions of users simultaneously.
3. High performance for searching and suggestions (< 500ms).

**Out of Scope:**
1. The system must recover quickly from failures.
2. Security.

## Core Models

- Admin
- User
- Movie
- Review
- Notification

## API Endpoints

### User

- POST /User/Register  
  Request:
  ```json
  { "username": "string", "email": "user@example.com", "password": "string" }
  ```

- POST /User/Login  
  Request:
  ```json
  { "email": "user@example.com", "password": "string" }
  ```

- GET /User/Profile  
  Request:
  ```json
  { "userId": 123 }
  ```

### Movie (Admin only)

- POST /Movie/Add  
  Request:
  ```json
  { "title": "string", "description": "string", "video": "url-or-path" }
  ```

- PUT /Movie/Manage  
  Request:
  ```json
  { "title": "string", "description": "string", "video": "url-or-path" }
  ```

### Movie (General)

- GET /Movie/Search → List<Movie>  
  Request:
  ```json
  { "title": "string" }
  ```

- POST /Movie/Review  
  Request:
  ```json
  { "MovieId": 1, "rating": 5, "comment": "Great movie!" }
  ```

- GET /Movie/Review  
  Request:
  ```json
  { "MovieId": 1 }
  ```
