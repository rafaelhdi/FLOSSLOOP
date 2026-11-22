-- =====================================================
-- FLOSSLOOP Database Setup Script (All-in-One)
-- =====================================================
-- Jalankan script ini SEKALI untuk setup lengkap:
--   1. Buat database FLOSSLOOP_db
--   2. Buat tabel users dengan data dummy
--   3. Buat tabel hasil_panen dengan data dummy
--
-- Cara jalankan:
--   psql -U postgres -f setup-database.sql
-- =====================================================

-- =====================================================
-- STEP 1: Buat Database
-- =====================================================
CREATE DATABASE IF NOT EXISTS FLOSSLOOP_db;

-- =====================================================
-- STEP 2: Hubungkan ke database FLOSSLOOP_db
-- =====================================================
\connect FLOSSLOOP_db

-- =====================================================
-- STEP 3: Buat Tabel Users (dengan data dummy)
-- =====================================================
CREATE TABLE IF NOT EXISTS public.users (
	id SERIAL PRIMARY KEY,
	nama VARCHAR(100) NOT NULL,
	email VARCHAR(150) UNIQUE NOT NULL,
	password VARCHAR(255) NOT NULL,
	role VARCHAR(20) NOT NULL
);

-- Insert data dummy untuk testing
INSERT INTO public.users (nama, email, password, role)
VALUES 
	('Admin Pemilik', 'pemilik@gmail.com', '123456', 'Pemilik'),
	('Karyawan', 'pegawai@gmail.com', '123456', 'Pegawai');

-- =====================================================
-- STEP 4: Buat Tabel Hasil Panen (dengan data dummy)
-- =====================================================
CREATE TABLE IF NOT EXISTS public.hasil_panen (
	id             SERIAL PRIMARY KEY,
	tanggal        DATE           NOT NULL,
	komoditas      VARCHAR(50)    NOT NULL,
	lokasi         VARCHAR(100)   NOT NULL,
	berat_panen    NUMERIC(10,2)  NOT NULL CHECK (berat_panen > 0),
	food_loss_kg   NUMERIC(10,2)  NOT NULL CHECK (food_loss_kg >= 0),
	created_by     INT REFERENCES public.users(id),
	created_at     TIMESTAMP      NOT NULL DEFAULT NOW(),
	CHECK (food_loss_kg <= berat_panen)
);

-- Insert data contoh
INSERT INTO public.hasil_panen (tanggal, komoditas, lokasi, berat_panen, food_loss_kg, created_by) VALUES
	('2026-11-10', 'Bayam',    'Blok A1', 120.0, 12.0, 2),
	('2026-11-11', 'Kangkung', 'Blok B2',  85.0,  8.5, 2),
	('2026-11-12', 'Tomat',    'Blok C1', 200.0, 22.0, 2),
	('2026-11-13', 'Cabai',    'Blok D3',  60.0,  6.0, 2);

-- =====================================================
-- SETUP COMPLETE
-- =====================================================
-- Database FLOSSLOOP_db sudah siap!
-- Login dengan:
--   Email: pemilik@gmail.com / Password: 123456
--   Email: pegawai@gmail.com / Password: 123456
