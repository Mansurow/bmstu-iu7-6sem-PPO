-- Index: IX_ZonePackages_zone_id

-- DROP INDEX IF EXISTS public."IX_ZonePackages_zone_id";

CREATE INDEX IF NOT EXISTS "IX_ZonePackages_zone_id"
    ON public."ZonePackages" USING btree
    (zone_id ASC NULLS LAST)
    TABLESPACE pg_default;

-- Index: IX_PackageDishes_package_id

-- DROP INDEX IF EXISTS public."IX_PackageDishes_package_id";

CREATE INDEX IF NOT EXISTS "IX_PackageDishes_package_id"
    ON public."PackageDishes" USING btree
    (package_id ASC NULLS LAST)
    TABLESPACE pg_default;
    
-- Index: IX_Inventories_zone_id

-- DROP INDEX IF EXISTS public."IX_Inventories_zone_id";

CREATE INDEX IF NOT EXISTS "IX_Inventories_zone_id"
    ON public."Inventories" USING btree
    (zone_id ASC NULLS LAST)
    TABLESPACE pg_default;    

-- Index: IX_Bookings_package_id

-- DROP INDEX IF EXISTS public."IX_Bookings_package_id";

CREATE INDEX IF NOT EXISTS "IX_Bookings_package_id"
    ON public."Bookings" USING btree
    (package_id ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_Bookings_user_id

-- DROP INDEX IF EXISTS public."IX_Bookings_user_id";

CREATE INDEX IF NOT EXISTS "IX_Bookings_user_id"
    ON public."Bookings" USING btree
    (user_id ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_Bookings_zone_id

-- DROP INDEX IF EXISTS public."IX_Bookings_zone_id";

CREATE INDEX IF NOT EXISTS "IX_Bookings_zone_id"
    ON public."Bookings" USING btree
    (zone_id ASC NULLS LAST)
    TABLESPACE pg_default;

-- Index: IX_Feedbacks_user_id

-- DROP INDEX IF EXISTS public."IX_Feedbacks_user_id";

CREATE INDEX IF NOT EXISTS "IX_Feedbacks_user_id"
    ON public."Feedbacks" USING btree
    (user_id ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: IX_Feedbacks_zone_id

-- DROP INDEX IF EXISTS public."IX_Feedbacks_zone_id";

CREATE INDEX IF NOT EXISTS "IX_Feedbacks_zone_id"
    ON public."Feedbacks" USING btree
    (zone_id ASC NULLS LAST)
    TABLESPACE pg_default;