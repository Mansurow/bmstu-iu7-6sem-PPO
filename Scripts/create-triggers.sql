-- Триггерная функция
CREATE OR REPLACE FUNCTION public.calculate_rating_zone()
RETURNS TRIGGER
AS $$
DECLARE 
   calc_rating RECORD;
   
BEGIN
    UPDATE public.zones
    SET rating = (Select sum(DISTINCT f.mark) / count(*) as rating 
                       from public.feedbacks as f
                       where f.zone_id = new.zone_id)
    WHERE id = new.zone_id;
    
    RETURN NEW;
END;
$$ LANGUAGE PLPGSQL;

-- Удаление триггера
-- DROP TRIGGER IF EXISTS insert_feedbacks_trigger on public."Feedbacks";

-- Создание триггера
CREATE TRIGGER insert_feedbacks_trigger
AFTER INSERT ON public.feedbacks
FOR EACH ROW
EXECUTE FUNCTION public.calculate_rating_zone();