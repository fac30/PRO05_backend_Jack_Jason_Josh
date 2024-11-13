CREATE TABLE colour (
  id SERIAL PRIMARY KEY,
  hex CHAR(6) NOT NULL, -- hex colour code, without the #
);

CREATE TABLE user (
  id SERIAL PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  email VARCHAR(255) UNIQUE NOT NULL,
  hash VARCHAR(255) NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP -- when the user was created
);

CREATE TABLE collection (
  -- id SERIAL PRIMARY KEY,
  -- type VARCHAR(255) NOT NULL,
  -- is_public BOOLEAN NOT NULL DEFAULT FALSE,
  -- user_id INTEGER REFERENCES user(id),
  name VARCHAR(255) NOT NULL,
  description TEXT,
  -- created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE colour_collection (
  -- id SERIAL PRIMARY KEY,
  -- colour_id INTEGER REFERENCES colour(id),
  -- collection_id INTEGER REFERENCES collection(id),
  order INTEGER NOT NULL -- to allow positioning of colours in a collection
);

CREATE TABLE comment (
  -- id SERIAL PRIMARY KEY,
  user_id INTEGER REFERENCES user(id),
  collection_id INTEGER REFERENCES collection(id),
  content TEXT NOT NULL, -- the actual comment text
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
