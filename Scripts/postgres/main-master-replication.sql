CREATE PUBLICATION main_master_pub_all
FOR ALL TABLES;

CREATE SUBSCRIPTION main_to_mirror_sub_all
  CONNECTION 'host=postgres_master_2 port=5432 user=repuser password=password123 dbname=PortalDb'
  PUBLICATION mirror_master_pub_all
  WITH (origin = none, copy_data = true);