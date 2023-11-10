CREATE ROLE repuser WITH REPLICATION LOGIN PASSWORD 'password123';
GRANT all ON all tables IN schema public TO repuser;