-- Table: public.Users

-- DROP TABLE IF EXISTS public."Users";

CREATE TABLE IF NOT EXISTS public."Users"
(
    id uuid NOT NULL,
    last_name character varying(64) NOT NULL,
    first_name character varying(64) NOT NULL,
    middle_name character varying(64),
    birthday character varying(64) NOT NULL,
    gender integer NOT NULL,
    email text NOT NULL,
    phone text,
    password character varying(128),
    role integer NOT NULL,
    
    CONSTRAINT "PK_Users" PRIMARY KEY (id)
);

-- Table: public.Zones

-- DROP TABLE IF EXISTS public."Zones";

CREATE TABLE IF NOT EXISTS public."Zones"
(
    id uuid NOT NULL,
    name character varying(64) NOT NULL,
    address text NOT NULL,
    size double precision NOT NULL,
    "limit" integer NOT NULL,
    price double precision NOT NULL,
    rating numeric NOT NULL,
    
    CONSTRAINT "PK_Zones" PRIMARY KEY (id)
);

-- Table: public.Inventories

-- DROP TABLE IF EXISTS public."Inventories";

CREATE TABLE IF NOT EXISTS public."Inventories"
(
    id uuid NOT NULL,
    zone_id uuid NOT NULL,
    name character varying(64) NOT NULL,
    description text NOT NULL,
    year_of_production date NOT NULL,
    is_written_off boolean NOT NULL,
    
    CONSTRAINT "PK_Inventories" PRIMARY KEY (id),
    CONSTRAINT "FK_Inventories_Zones_zone_id" FOREIGN KEY (zone_id)
        REFERENCES public."Zones" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

-- Table: public.Packages

-- DROP TABLE IF EXISTS public."Packages";

CREATE TABLE IF NOT EXISTS public."Packages"
(
    id uuid NOT NULL,
    name character varying(64) NOT NULL,
    type integer NOT NULL,
    price double precision NOT NULL,
    rental_time integer NOT NULL,
    description text NOT NULL,
    
    CONSTRAINT "PK_Packages" PRIMARY KEY (id)
);

-- Table: public.ZonePackages

-- DROP TABLE IF EXISTS public."ZonePackages";

CREATE TABLE IF NOT EXISTS public."ZonePackages"
(
    package_id uuid NOT NULL,
    zone_id uuid NOT NULL,
    
    CONSTRAINT "PK_ZonePackages" PRIMARY KEY (package_id, zone_id),
    CONSTRAINT "FK_ZonePackages_Packages_package_id" FOREIGN KEY (package_id)
        REFERENCES public."Packages" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    
    CONSTRAINT "FK_ZonePackages_Zones_zone_id" FOREIGN KEY (zone_id)
        REFERENCES public."Zones" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

-- Table: public.Menu

-- DROP TABLE IF EXISTS public."Menu";

CREATE TABLE IF NOT EXISTS public."Menu"
(
    id uuid NOT NULL,
    name character varying(64) NOT NULL,
    type character varying(64) NOT NULL,
    price double precision NOT NULL,
    description text NOT NULL,
    CONSTRAINT "PK_Menu" PRIMARY KEY (id)
);

-- Table: public.PackageDishes

-- DROP TABLE IF EXISTS public."PackageDishes";

CREATE TABLE IF NOT EXISTS public."PackageDishes"
(
    dish_id uuid NOT NULL,
    package_id uuid NOT NULL,
    
    CONSTRAINT "PK_PackageDishes" PRIMARY KEY (dish_id, package_id),
    CONSTRAINT "FK_PackageDishes_Menu_dish_id" FOREIGN KEY (dish_id)
        REFERENCES public."Menu" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    
    CONSTRAINT "FK_PackageDishes_Packages_package_id" FOREIGN KEY (package_id)
        REFERENCES public."Packages" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

-- Table: public.Feedbacks

-- DROP TABLE IF EXISTS public."Feedbacks";

CREATE TABLE IF NOT EXISTS public."Feedbacks"
(
    id uuid NOT NULL,
    user_id uuid NOT NULL,
    zone_id uuid NOT NULL,
    date character varying(64) NOT NULL,
    mark double precision NOT NULL,
    
    message text COLLATE pg_catalog."default",
    CONSTRAINT "PK_Feedbacks" PRIMARY KEY (id),
    CONSTRAINT "FK_Feedbacks_Users_user_id" FOREIGN KEY (user_id)
        REFERENCES public."Users" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    
    CONSTRAINT "FK_Feedbacks_Zones_zone_id" FOREIGN KEY (zone_id)
        REFERENCES public."Zones" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

-- Table: public.Bookings

-- DROP TABLE IF EXISTS public."Bookings";

CREATE TABLE IF NOT EXISTS public."Bookings"
(
    id uuid NOT NULL,
    zone_id uuid NOT NULL,
    user_id uuid NOT NULL,
    package_id uuid NOT NULL,
    amount_of_people integer NOT NULL,
    status integer NOT NULL,
    date date NOT NULL,
    start_time time without time zone NOT NULL,
    end_time time without time zone NOT NULL,
    
    CONSTRAINT "PK_Bookings" PRIMARY KEY (id),
    CONSTRAINT "FK_Bookings_Packages_package_id" FOREIGN KEY (package_id)
        REFERENCES public."Packages" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    
    CONSTRAINT "FK_Bookings_Users_user_id" FOREIGN KEY (user_id)
        REFERENCES public."Users" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    
    CONSTRAINT "FK_Bookings_Zones_zone_id" FOREIGN KEY (zone_id)
        REFERENCES public."Zones" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);