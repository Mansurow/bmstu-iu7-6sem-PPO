CREATE PUBLICATION mirror_master_pub_all
FOR ALL TABLES;

CREATE SUBSCRIPTION mirior_to_main_sub_all
  CONNECTION 'host=postgres_master_1 port=5432 user=repuser password=password123 dbname=PortalDb'
  PUBLICATION main_master_pub_all
  WITH (origin = none, copy_data = true);