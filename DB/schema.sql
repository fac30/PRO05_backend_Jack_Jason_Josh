CREATE TABLE user (
  id SERIAL PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  email VARCHAR(255) UNIQUE NOT NULL,
  hash VARCHAR(255) NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP -- when the user was created
);

CREATE TABLE colour (
  id SERIAL PRIMARY KEY,
  hex CHAR(6) NOT NULL -- hex colour code, without the #
);

CREATE TABLE collection (
  id SERIAL PRIMARY KEY,
  type VARCHAR(255) NOT NULL, -- "favourite" or "collection", but I've made this a string rather than a bool so we can add more types in the future
  user_id INTEGER REFERENCES user(id),
  name VARCHAR(255) NOT NULL,
  description TEXT -- user-created blurb
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE comment (
  id SERIAL PRIMARY KEY,
  user_id INTEGER REFERENCES user(id),
  collection_id INTEGER REFERENCES collection(id),
  content TEXT NOT NULL -- the actual comment text
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE colour_collection (
  id SERIAL PRIMARY KEY,
  colour_id INTEGER REFERENCES colour(id),
  collection_id INTEGER REFERENCES collection(id),
  order INTEGER NOT NULL -- to allow positioning of colours in a collection
);
