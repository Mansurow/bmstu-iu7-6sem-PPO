-- Table: public.users

-- DROP TABLE IF EXISTS public.users;

CREATE TABLE IF NOT EXISTS public.users
(
    id uuid NOT NULL PRIMARY KEY,
    last_name varchar(64) NOT NULL,
    first_name varchar(64) NOT NULL,
    middle_name varchar(64),
    birthday timestamp with time zone NOT NULL,
    gender integer NOT NULL,
    email varchar(64) NOT NULL,
    phone varchar(64),
    password character varying(128) NOT NULL,
    role character varying(64) NOT NULL
);

-- Table: public.zones

-- DROP TABLE IF EXISTS public.zones;

CREATE TABLE IF NOT EXISTS public.zones
(
    id uuid NOT NULL PRIMARY KEY,
    name varchar(64) NOT NULL,
    address text NOT NULL,
    size double precision NOT NULL,
    "limit" integer NOT NULL,
    rating numeric NOT NULL
);

-- Table: public.inventories

-- DROP TABLE IF EXISTS public.inventories;

CREATE TABLE IF NOT EXISTS public.inventories
(
    id uuid NOT NULL PRIMARY KEY ,
    zone_id uuid NOT NULL REFERENCES public.zones (id) 
        MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    name varchar(64) NOT NULL,
    description text NOT NULL,
    date_production date NOT NULL,
    is_written_off boolean NOT NULL      
);

-- Table: public.packages

-- DROP TABLE IF EXISTS public.packages;

CREATE TABLE IF NOT EXISTS public.packages
(
    id uuid NOT NULL PRIMARY KEY,
    name varchar(64) NOT NULL,
    type varchar(64) NOT NULL,
    price numeric NOT NULL,
    rental_time integer NOT NULL,
    description text NOT NULL
);

-- Table: public.ZonePackages

-- DROP TABLE IF EXISTS public.zone_packages;

CREATE TABLE IF NOT EXISTS public.zone_packages
(
    package_id uuid NOT NULL
        REFERENCES public.zones (id) 
        MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    zone_id uuid NOT NULL 
        REFERENCES public.packages (id)
        MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    
    CONSTRAINT pk_zone_packages PRIMARY KEY (package_id, zone_id)
);

-- Table: public.dishes

-- DROP TABLE IF EXISTS public.dishes;

CREATE TABLE IF NOT EXISTS public.dishes
(
    id uuid NOT NULL PRIMARY KEY,
    name varchar(64) NOT NULL,
    type varchar(64) NOT NULL,
    price numeric NOT NULL,
    description text NOT NULL
);

-- Table: public.package_dishes

-- DROP TABLE IF EXISTS public.package_dishes;

CREATE TABLE IF NOT EXISTS public.package_dishes
(
    dish_id uuid NOT NULL 
        REFERENCES public.dishes (id) 
        MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    package_id uuid NOT NULL
        REFERENCES public.packages (id) 
        MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    
    CONSTRAINT pk_package_dishes PRIMARY KEY (dish_id, package_id)
);

-- Table: public.feedbacks

-- DROP TABLE IF EXISTS public.feedbacks;

CREATE TABLE IF NOT EXISTS public.feedbacks
(
    id uuid NOT NULL PRIMARY KEY,
    user_id uuid NOT NULL
        REFERENCES public.users (id) 
        MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    zone_id uuid NOT NULL 
        REFERENCES public.zones (id) 
        MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    date timestamp with time zone NOT NULL,
    mark numeric NOT NULL,
    message text
);

-- Table: public.bookings

-- DROP TABLE IF EXISTS public.bookings;

CREATE TABLE IF NOT EXISTS public.bookings
(
    id uuid NOT NULL PRIMARY KEY,
    zone_id uuid NOT NULL
        REFERENCES public.zones (id) 
        MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    user_id uuid NOT NULL
        REFERENCES public.users (id) 
        MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    package_id uuid NOT NULL
        REFERENCES public.packages (id) 
        MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    amount_of_people integer NOT NULL,
    status varchar(64) NOT NULL,
    date date NOT NULL,
    start_time time without time zone NOT NULL,
    end_time time without time zone NOT NULL,
    create_date_time timestamp with time zone NOT NULL,
    is_paid boolean NOT NULL,
    total_price numeric NOT NULL
);