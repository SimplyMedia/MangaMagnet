import { Inter } from 'next/font/google'
import { ChangeEvent, useEffect, useState } from "react";
import { MangaCard } from "@/components/manga/MangaCard";
import { Layout } from "@/components/layout/Layout";
import { MangaResponse, MangaService, MangaStatus } from "@/services/openapi";

const inter = Inter({subsets: ['latin']})

export default function Home() {
	const [mangaList, setMangaList] = useState<MangaResponse[]>([]);

	useEffect(() => {
		MangaService.mangaGetAll()
			.then(data => setMangaList(data))
			.catch(error => console.error('Error fetching data:', error));
	}, []);

	const [filteredMangaList, setFilteredMangaList] = useState<MangaResponse[]>([]);
	const [statusFilter, setStatusFilter] = useState<MangaStatus | ''>('')

	const onStatusFilterChange = (e: ChangeEvent<HTMLSelectElement>) => {
		setStatusFilter(e.target.value as MangaStatus | '')
	}
	useEffect(() => {
		if (statusFilter === '') {
			setFilteredMangaList(mangaList);
		} else {
			setFilteredMangaList(mangaList.filter(i => i.metadata.status === statusFilter));
		}
	}, [mangaList, statusFilter]);

	return (
		<Layout>
			<select className={"m-4 bg-black"} value={statusFilter} onChange={onStatusFilterChange}>
				<option value={''}>No filter</option>
				{Object.values(MangaStatus).filter(i => isNaN(parseInt(i))).map(i => (
					<option key={i} value={i}>{i}</option>
				))}
			</select>

			{filteredMangaList.length === 0 && (
				<div className={"m-4 text-center"}>
					{statusFilter
						? "There is no manga found with the selected filter"
						: "There is no manga in your library (yet)"}
				</div>
			)}

			<div className={"flex flex-wrap overflow-y-auto m-4"}>
				{filteredMangaList.map(manga => (
					<MangaCard key={manga.id} manga={manga}/>
				))}
			</div>
		</Layout>
	)
}
